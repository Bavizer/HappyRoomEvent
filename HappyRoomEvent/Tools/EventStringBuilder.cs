using HappyRoomEvent.Core.SubEvents;
using LabApi.Features.Wrappers;
using UnityEngine;

namespace HappyRoomEvent.Tools;

internal static class EventStringBuilder
{
    private static EventStrings EventStrings => Core.HappyRoomEvent.Instance.Config.EventStrings;

    internal static string? GetEventStatusString(SubEvent? activeSubEvent, float timerValue)
    {
        string? message;

        if (activeSubEvent is null)
            message = timerValue > 0f ? GetSubEventAwaitingString(timerValue) : null;
        else
            message = activeSubEvent.HasStarted 
                ? GetSubEventActiveString(activeSubEvent.RemainingTime) 
                : activeSubEvent.Settings.Description;

        return message;
    }

    internal static string GetEventEndString(Player? winner)
    {
        var winnerString = winner is null ? "undefined" : winner.Nickname;
        return EventStrings.EventEnd.Replace("{winner}", winnerString);
    }

    private static string GetSubEventAwaitingString(float remainingTime) =>
        EventStrings.SubEventAwaiting.Replace("{time}", Mathf.RoundToInt(remainingTime).ToString());

    private static string GetSubEventActiveString(float remainingTime) =>
        EventStrings.SubEventActive.Replace("{time}", Mathf.RoundToInt(remainingTime).ToString());
}
