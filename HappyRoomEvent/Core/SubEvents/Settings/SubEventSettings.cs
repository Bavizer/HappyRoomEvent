namespace HappyRoomEvent.Core.SubEvents.Settings;

public class SubEventSettings
{
    public const float MinSubEventDuration = 10f;

    public virtual string Description { get; set; } = string.Empty;

    public virtual float IntroDuration
    {
        get;
        set => field = value > 0 ? value : 0;
    } = 5f;

    public virtual float ActiveDuration
    {
        get;
        set => field = value > MinSubEventDuration ? value : MinSubEventDuration;
    } = 60f;
}
