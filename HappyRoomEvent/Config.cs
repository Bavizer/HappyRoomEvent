using HappyRoomEvent.Core.SubEvents.Settings;
using System.ComponentModel;
using UnityEngine;

namespace HappyRoomEvent;

internal sealed class Config
{
    [Description("Where the room will be spawned when the event starts")]
    public Vector3 RoomPosition { get; set; } = new(-100, 225, 50);

    [Description("Disable round lock after event ending? (true/false)")]
    public bool DisableRoundLockOnEnd { get; set; } = false;

    [Description("Sub-Events' cooldown duration before starting a new SubEvent")]
    public float SubEventsCooldownDuration
    {
        get;
        set => field = value > 0 ? value : 0;
    } = 5f;

    public SubEventsSettings SubEventsSettings { get; set; } = new();

    public EventStrings EventStrings { get; set; } = new();
}

internal struct SubEventsSettings
{
    public DiscoSubEventSettings DiscoSubEventSettings { get; set; } = new()
    {
        Description = 
            "<b>Sub-Event \"<color=red>D<color=orange>i<color=yellow>s<color=green>c<color=#00ffffff>o<color=white>\"\n" +
            "Guess and stay on the <u>right color</u></b>"
    };

    public ErythrophobiaSubEventSettings ErythrophobiaSubEventSettings { get; set; } = new()
    {
        Description = "<b>Sub-Event <color=red>\"Erythrophobia\"</color>\nStay away from <color=red>red color</color></b>",
    };

    public SubEventSettings LavaSubEventSettings { get; set; } = new()
    {
        IntroDuration = 10f,
        Description = "<b>Sub-Event <color=orange>\"Lava\"</color>\nGet to the top until it's <color=red>too late</color></b>"
    };

    public SubEventsSettings() { }
}

internal struct EventStrings
{
    public string SubEventAwaiting { get; set; } =
        "<b>Next Sub-Event in <color=orange>{time} s.</color></b>";

    public string SubEventActive { get; set; } =
        "<b>Sub-Event remaining time: <color=orange>{time}</color></b>";

    public string EventEnd { get; set; } =
        "<b>Event <color=orange>\"Happy Room\"</color> has been ended\nWinner is <color=red>{winner}</color></b>";

    public EventStrings() { }
}
