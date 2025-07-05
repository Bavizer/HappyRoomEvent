using AdminToys;
using HappyRoomEvent.Core.Components.SubEvents;
using HappyRoomEvent.Core.SubEvents.Settings;
using HappyRoomEvent.Extensions;
using MEC;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HappyRoomEvent.Core.SubEvents;

public class DiscoSubEvent : SubEvent<DiscoSubEventSettings>
{
    private readonly PrimitiveObjectToy[] _primitives;
    private readonly List<DangerColorFloor> _floors;

    private readonly Color _unassignedColor = Color.white;

    private readonly Color[] _floorColors =
    [
        Color.blue, 
        Color.red, 
        Color.cyan, 
        Color.magenta, 
        Color.yellow, 
        new(0f, 0.75f, 0f),
        new(1f, 0.5f, 0f), 
        new(0f, 1f, 0.5f),
    ];

    private Color _safeColor;

    internal DiscoSubEvent(HappyRoom happyRoom, DiscoSubEventSettings settings) : base(happyRoom, settings)
    {
        Transform @event = happyRoom.Schematic.transform.Find("SubEvents/DiscoEvent");
        _primitives = @event.GetNonDefaultPrimitivesInChildren();

        Transform floorsParent = @event.Find("Floors");
        _floors = new List<DangerColorFloor>(floorsParent.childCount);

        foreach (Transform obj in floorsParent)
        {
            var floor = obj.gameObject.AddComponent<DangerColorFloor>();
            floor.Init(_unassignedColor, Color.black, Settings.SwitchToDangerDuration);
            _floors.Add(floor);
        }
    }

    protected override void Reset()
    {
        base.Reset();
        _floors.ForEach(hf => hf.Reset());
    }

    protected override void SetActive(bool value)
    {
        var flags = value ? PrimitiveFlags.Collidable | PrimitiveFlags.Visible : PrimitiveFlags.None;
        _primitives.ForEach(p => p.NetworkPrimitiveFlags = flags);
        _floors.ForEach(f => f.enabled = value);

        if (value)
            RandomAssignFloorColors();
    }

    protected override IEnumerator<float> Update()
    {
        yield return Timing.WaitForOneFrame;

        while (!AreEndConditionsCompleted)
        {
            yield return Timing.WaitForSeconds(1f);
            _floors.ForEach(f => f.Reset());

            _safeColor = _floorColors.RandomValue();

            var isReadyToSwitch = false;
            Timing.CallPeriodically(Settings.RandomColorSwitchingOnStartDuration, 0.2f, 
                RandomAssignFloorColors, () => isReadyToSwitch = true);
            yield return Timing.WaitUntilTrue(() => isReadyToSwitch);

            _floors.ForEach(f => f.ShouldSwitchToDangerous = f.DefaultColor != _safeColor);
            yield return Timing.WaitForSeconds(Settings.SwitchToDangerDuration);
        }
    }

    private void RandomAssignFloorColors()
    {
        _floors.ForEach(f => f.DefaultColor = _unassignedColor);

        foreach (var color in _floorColors)
        {
            var floorsPerColor = _floors.Count / _floorColors.Length;

            for (int i = 0; i < floorsPerColor; i++)
            {
                DangerColorFloor? randomFloor = _floors.RandomValue(p => p.DefaultColor == _unassignedColor);
                if (randomFloor is null)
                    return;

                randomFloor.DefaultColor = color;
            }
        }
    }
}
