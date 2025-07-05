using UnityEngine;

namespace HappyRoomEvent.Core.Components.DynamicObjects;

[DisallowMultipleComponent]
public class RotationComponent : DynamicObjectComponent
{
    protected override void Update()
    {
        Duration -= Time.deltaTime;
        transform.Rotate(Axis, Speed * Time.deltaTime);
    }
}
