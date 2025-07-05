using HappyRoomEvent.Core.SubEvents.Settings;
using HappyRoomEvent.Tools;
using MEC;
using System;

namespace HappyRoomEvent.Core.SubEvents;

public abstract class SubEvent : Event
{
    #region Fields

    private bool _isActive;

    protected readonly CountdownTimer Timer = new();

    protected readonly HappyRoom HappyRoom;

    #endregion

    #region Properties

    public override bool IsActive => _isActive;

    public bool HasStarted => IsActive && Timer.IsRunning;

    public float RemainingTime => Timer.RemainingTime;

    public sealed override bool AreEndConditionsCompleted => RemainingTime <= 0f;

    public SubEventSettings Settings { get; }

    #endregion

    #region Methods

    protected SubEvent(HappyRoom happyRoom, SubEventSettings settings)
    {
        HappyRoom = happyRoom;
        Settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    public sealed override void StartEvent() => HappyRoomEvent.Instance.StartSubEvent(this);

    public sealed override void EndEvent() => HappyRoomEvent.Instance.EndSubEvent(this);

    internal void InternalStartEvent()
    {
        if (IsActive)
            throw new InvalidOperationException("Cannot start Sub-Event: it's already active.");

        _isActive = true;

        Reset();
        SetActive(true);

        Timing.CallDelayed(Settings.IntroDuration, () =>
        {
            Timer.Start();
            base.StartEvent();
        });
    }

    internal void InternalEndEvent()
    {
        if (!IsActive)
            throw new InvalidOperationException("Cannot end Sub-Event: it's not active.");

        base.EndEvent();
        SetActive(false);
        Reset();
        Timer.Stop();

        _isActive = false;
    }

    protected virtual void Reset() => Timer.RemainingTime = Settings.ActiveDuration;

    protected abstract void SetActive(bool value);

    #endregion
}

public abstract class SubEvent<TSettings> : SubEvent where TSettings : SubEventSettings
{
    public new TSettings Settings => (TSettings)base.Settings;

    protected SubEvent(HappyRoom happyRoomSchematic, TSettings settings) : base(happyRoomSchematic, settings) { }
}
