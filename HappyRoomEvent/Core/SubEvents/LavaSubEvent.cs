using AdminToys;
using HappyRoomEvent.Core.Components.DynamicObjects;
using HappyRoomEvent.Core.Components.SubEvents;
using HappyRoomEvent.Core.SubEvents.Settings;
using HappyRoomEvent.Extensions;
using MEC;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HappyRoomEvent.Core.SubEvents;

public class LavaSubEvent : SubEvent
{
    #region Fields

    private readonly PrimitiveObjectToy[] _primitives;
    private readonly DynamicObjectComponent[] _dynamicObjects;
    private readonly Lava _lava;

    private readonly LightSourceToy _eventLight;

    private readonly Bounds _safePointBounds;

    #endregion

    #region Methods

    internal LavaSubEvent(HappyRoom happyRoom, SubEventSettings settings) : base(happyRoom, settings)
    {
        Transform @event = happyRoom.Schematic.transform.Find("SubEvents/LavaEvent");
        _primitives = @event.GetNonDefaultPrimitivesInChildren();
        _dynamicObjects = @event.GetComponentsInChildren<DynamicObjectComponent>();

        _lava = @event.Find("Lava").gameObject.AddComponent<Lava>();
        _lava.Init(@event.Find("LavaTargetScale").localScale, Settings.ActiveDuration);

        _eventLight = @event.Find("EventLight").GetComponent<LightSourceToy>();

        Transform safePoint = @event.Find("SafePoint");
        _safePointBounds = new Bounds(safePoint.position, safePoint.localScale);
    }

    protected override void Reset()
    {
        base.Reset();
        _lava.Reset();
    }

    protected override void SetActive(bool value)
    {
        var flags = value ? PrimitiveFlags.Collidable | PrimitiveFlags.Visible : PrimitiveFlags.None;
        _primitives.ForEach(p => p.NetworkPrimitiveFlags = flags);

        HappyRoom.RoomLight.NetworkLightIntensity = value ? 0f : 100f;
        _eventLight.NetworkLightIntensity = value ? 150f : 0f;

        _dynamicObjects.ForEach(so => so.enabled = value);

        HappyRoomEvent.Instance.Players.ForEach(p => p.Position = HappyRoom.SpawnPoint.position);

        if (value)
            Timing.CallDelayed(Settings.IntroDuration, () => _lava.enabled = IsActive);
        else
            _lava.enabled = value;
    }

    protected override IEnumerator<float> Update()
    {
        yield return Timing.WaitForOneFrame;

        while (!AreEndConditionsCompleted)
        {
            if (HappyRoomEvent.Instance.Players.All(p => _safePointBounds.Contains(p.Position)))
                EndEvent();

            yield return Timing.WaitForSeconds(1f);
        }
    }

    #endregion
}
