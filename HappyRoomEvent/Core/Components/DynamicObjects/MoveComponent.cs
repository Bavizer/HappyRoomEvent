using UnityEngine;

namespace HappyRoomEvent.Core.Components.DynamicObjects;

[DisallowMultipleComponent]
public class MoveComponent : DynamicObjectComponent
{
    protected override void Update()
    {
        Duration -= Time.deltaTime;
        Vector3 position = transform.position;
        transform.position = Vector3.MoveTowards(position, position + Axis, Speed * Time.deltaTime);
    }
}
