using CustomPlayerEffects;
using CustomRendering;
using GameCore;
using HappyRoomEvent.Core.SubEvents;
using HappyRoomEvent.Extensions;
using HappyRoomEvent.Tools;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HappyRoomEvent.Core;

public sealed class HappyRoomEvent : Event
{
    #region Fields

    private readonly List<SubEvent> _subEvents = [];
    private readonly List<SubEvent> _notRepeatedSubEvents = [];

    private readonly CustomEventsHandler _eventsHandler = new EventsHandler();

    private readonly CountdownTimer _subEventSelectionTimer = new();

    private CoroutineHandle _selectSubEventCoroutine;
    private CoroutineHandle _sendBroadcastCoroutine;

    private HappyRoom? _happyRoom;

    public const ushort MinimumPlayersRequired = 2;
    public const RoleTypeId PlayersRoleType = RoleTypeId.ClassD;

    #endregion

    #region Properties

    internal Config Config => Plugin.Instance.Config!;

    public static HappyRoomEvent Instance => field ??= new HappyRoomEvent();

    public SubEvent? ActiveSubEvent { get; private set; }

    public override bool AreEndConditionsCompleted => Players.Count() < MinimumPlayersRequired;

    public IEnumerable<Player> Players => Player.ReadyList.Where(p => p.Role == PlayersRoleType);

    #endregion

    #region Methods

    private HappyRoomEvent() { }

    public override void StartEvent()
    {
        var eventName = nameof(HappyRoomEvent);

        if (IsActive)
            throw new InvalidOperationException($"Cannot start {eventName}: it's already active.");

        if (Players.Count() < MinimumPlayersRequired)
            throw new InvalidOperationException($"Minimum {MinimumPlayersRequired} players required to start {eventName}.");

        if (!Round.IsRoundInProgress)
            throw new InvalidOperationException($"Cannot start {eventName}: round has to be started.");

        try
        {
            Initialize();
            ApplyEventSettingsForServerAndRound(true);

            base.StartEvent();
            _selectSubEventCoroutine = Timing.RunCoroutine(SelectSubEvent());
            _sendBroadcastCoroutine = Timing.RunCoroutine(SendBroadcast());
        }
        catch
        {
            Clear();
            throw;
        }
    }

    public override void EndEvent()
    {
        if (!IsActive)
            throw new InvalidOperationException($"Cannot end {nameof(HappyRoomEvent)}: it's not active.");

        try
        {
            OnEnd();
        }
        finally
        {
            Clear();
            Logger.Info("Event ended.");
        }
    }

    public void StartSubEvent(SubEvent subEvent)
    {
        if (ActiveSubEvent is not null)
            throw new InvalidOperationException("Cannot start Sub-Event: another Sub-Event is already active.");

        ActiveSubEvent = subEvent ?? throw new ArgumentNullException(nameof(subEvent));
        subEvent.InternalStartEvent();
    }

    public void EndSubEvent(SubEvent subEvent)
    {
        if (subEvent is null)
            throw new ArgumentNullException(nameof(subEvent));

        if (ActiveSubEvent is null || ActiveSubEvent != subEvent)
            throw new InvalidOperationException("Cannot end Sub-Event: it's not active.");

        subEvent.InternalEndEvent();
        ActiveSubEvent = null;
    }

    private void Initialize()
    {
        Plugin.Instance.LoadAndSaveConfig();

        CustomHandlersManager.RegisterEventsHandler(_eventsHandler);

        _happyRoom = new HappyRoom(Config.RoomPosition, Quaternion.identity);

        SubEvent[] subEvents =
        [
            new DiscoSubEvent(_happyRoom.Value, Config.SubEventsSettings.DiscoSubEventSettings),
            new ErythrophobiaSubEvent(_happyRoom.Value, Config.SubEventsSettings.ErythrophobiaSubEventSettings),
            new LavaSubEvent(_happyRoom.Value, Config.SubEventsSettings.LavaSubEventSettings)
        ];
        _subEvents.AddRange(subEvents);
    }

    private void Clear()
    {
        CustomHandlersManager.UnregisterEventsHandler(_eventsHandler);
        Timing.KillCoroutines(_sendBroadcastCoroutine, _selectSubEventCoroutine);
        base.EndEvent();

        _subEvents.Where(se => se.IsActive).ForEach(se => se.EndEvent());
        ApplyEventSettingsForServerAndRound(false);

        _happyRoom?.Schematic.Destroy();
        _happyRoom = null;

        _subEvents.Clear();
        _notRepeatedSubEvents.Clear();

        _subEventSelectionTimer.Stop();
    }

    private SubEvent StartRandomSubEvent()
    {
        if (!_notRepeatedSubEvents.Any())
            _notRepeatedSubEvents.AddRange(_subEvents);

        var subEvent = _notRepeatedSubEvents.PullRandomItem();
        subEvent.StartEvent();
        return subEvent;
    }

    private void EnsurePlayerIsInRoom(Player player)
    {
        if (!_happyRoom?.Bounds.Contains(player.Position) ?? false)
            player.Position = _happyRoom!.Value.SpawnPoint.position;
    }

    private void ApplyEventSettingsForServerAndRound(bool isEventActive)
    {
        Server.FriendlyFire = isEventActive || ConfigFile.ServerConfig.GetBool("friendly_fire");
        Round.IsLocked = isEventActive || !Config.DisableRoundLockOnEnd;
    }

    protected override IEnumerator<float> Update()
    {
        yield return Timing.WaitForOneFrame;

        while (!AreEndConditionsCompleted)
        {
            Players.ForEach(EnsurePlayerIsInRoom);

            foreach (var player in Player.ReadyList)
                player.GetEffect<FogControl>()?.SetFogType(FogType.None);

            yield return Timing.WaitForSeconds(1f);
        }
    }

    private IEnumerator<float> SelectSubEvent()
    {
        yield return Timing.WaitForOneFrame;

        while (!AreEndConditionsCompleted)
        {
            yield return Timing.WaitUntilFalse(() => ActiveSubEvent?.IsActive ?? false);

            _subEventSelectionTimer.RemainingTime = Config.SubEventsCooldownDuration;
            _subEventSelectionTimer.Start();

            yield return Timing.WaitUntilFalse(() => _subEventSelectionTimer.RemainingTime > 0f);
            StartRandomSubEvent();
        }
    }

    private IEnumerator<float> SendBroadcast()
    {
        yield return Timing.WaitForOneFrame;

        while (!AreEndConditionsCompleted)
        {
            string? message = EventStringBuilder.GetEventStatusString(ActiveSubEvent, _subEventSelectionTimer.RemainingTime);
            if (message is not null)
                Server.SendBroadcast(message, 1, shouldClearPrevious: true);

            yield return Timing.WaitForSeconds(0.9f);
        }
    }

    private void OnEnd()
    {
        Player? winner = Players.Count() <= 1 ? Players.SingleOrDefault() : null;

        foreach (var player in Player.ReadyList)
        {
            if (!player.IsOverwatchEnabled)
                player.SetRole(RoleTypeId.Spectator);
        }
        winner?.SetRole(RoleTypeId.Tutorial);

        Logger.Info($"Winner is {(winner is null ? "undefined" : winner.Nickname)}");
        Server.SendBroadcast(EventStringBuilder.GetEventEndString(winner), 10, shouldClearPrevious: true);
    }

    #endregion
}
