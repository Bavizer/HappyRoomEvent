namespace HappyRoomEvent.Core.SubEvents.Settings;

public class DiscoSubEventSettings : SubEventSettings
{
    public float SwitchToDangerDuration
    {
        get;
        set => field = value > 0 ? value : 0;
    } = 4f;

    public float RandomColorSwitchingOnStartDuration
    {
        get;
        set => field = value > 0 ? value : 1f;
    } = 1f;
}

