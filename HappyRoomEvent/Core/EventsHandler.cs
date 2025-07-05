using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.CustomHandlers;

namespace HappyRoomEvent.Core;

internal class EventsHandler : CustomEventsHandler
{
    private static HappyRoomEvent Event => HappyRoomEvent.Instance;

    public override void OnServerRoundRestarted() => Event.EndEvent();

    public override void OnServerWaveRespawning(WaveRespawningEventArgs ev) => ev.IsAllowed = false;

    public override void OnServerWaveTeamSelecting(WaveTeamSelectingEventArgs ev) => ev.IsAllowed = false;
}
