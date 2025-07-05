using System;
using AdminToys;
using HappyRoomEvent.Core.Components.SubEvents;
using HappyRoomEvent.Core.SubEvents.Settings;
using HappyRoomEvent.Extensions;
using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace HappyRoomEvent.Core.SubEvents;

public class ErythrophobiaSubEvent : SubEvent<ErythrophobiaSubEventSettings>
{
    #region Fields

    private readonly PrimitiveObjectToy[] _primitives;
    private readonly List<DangerColorFloor> _floors;
    private readonly DeathRotor _deathRotor;

    #endregion

    #region Methods

    internal ErythrophobiaSubEvent(HappyRoom happyRoom, ErythrophobiaSubEventSettings settings) : base(happyRoom, settings)
    {
        Transform @event = happyRoom.Schematic.transform.Find("SubEvents/ErythrophobiaEvent");
        _primitives = @event.GetNonDefaultPrimitivesInChildren();

        _deathRotor = @event.Find("Rotor").gameObject.AddComponent<DeathRotor>();
        _deathRotor.Init(0f, settings.RotorMaxAngularSpeed, settings.RotorAcceleration);

        Transform floorsParent = @event.Find("Floors");
        float switchDuration = settings.ActiveDuration / (floorsParent.childCount - 1);
        _floors = new List<DangerColorFloor>(floorsParent.childCount);

        foreach (Transform obj in floorsParent)
        {
            var floor = obj.gameObject.AddComponent<DangerColorFloor>();
            floor.Init(Color.green, Color.red, switchDuration);
            _floors.Add(floor);
        }
    }

    protected override void Reset()
    {
        base.Reset();
        _deathRotor.Reset();
        _floors.ForEach(hf => hf.Reset());
    }

    protected override void SetActive(bool value)
    {
        var flags = value ? PrimitiveFlags.Collidable | PrimitiveFlags.Visible : PrimitiveFlags.None;
        _primitives.ForEach(p => p.NetworkPrimitiveFlags = flags);
        _deathRotor.PrimitiveFlags = value ? PrimitiveFlags.Visible : PrimitiveFlags.None;

        _floors.ForEach(hf => hf.enabled = value);

        if (value)
            Timing.CallDelayed(Settings.IntroDuration, () => _deathRotor.enabled = IsActive);
        else
            _deathRotor.enabled = value;
    }

    protected override IEnumerator<float> Update()
    {
        yield return Timing.WaitForOneFrame;

        while (!AreEndConditionsCompleted)
        {
            DangerColorFloor? randomFloor = _floors.RandomValue(o => !o.IsDangerous);
            if (randomFloor is null)
                yield break;

            randomFloor.ShouldSwitchToDangerous = true;
            yield return Timing.WaitUntilTrue(() => randomFloor.IsDangerous);
        }
    }

    #endregion
}
