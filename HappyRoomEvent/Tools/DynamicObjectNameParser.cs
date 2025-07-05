using HappyRoomEvent.Core.Components.DynamicObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace HappyRoomEvent.Tools;

internal static class DynamicObjectNameParser
{
    private static readonly Dictionary<Type, string> ComponentPatterns = new()
    {
        { typeof(MoveComponent), @"^Move:(-?\d+)$" },
        { typeof(RotationComponent), @"^Rotation:(-?\d+)$" },
        { typeof(ScaleComponent), @"^Scale:(-?\d+)$" }
    };

    public static readonly string ObjectPattern = 
        $@"^\[{"DynamicObject"}\]\s*\{{(.+)\}}$";

    public static void ParseFromName(GameObject obj)
    {
        var objectMatch = Regex.Match(obj.name, ObjectPattern);
        if (!objectMatch.Success)
            return;

        string content = objectMatch.Groups[1].Value;
        var tokens = content.Split([','], StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim());

        foreach (var token in tokens)
        {
            foreach (var pair in ComponentPatterns)
            {
                Type type = pair.Key;
                string pattern = pair.Value;

                var match = Regex.Match(token, pattern);
                if (!match.Success)
                    continue;

                var value = float.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                var component = (DynamicObjectComponent)(obj.GetComponent(type) ?? obj.AddComponent(type));
                component.Init(float.PositiveInfinity, Vector3.forward * value);

                break;
            }
        }
    }
}
