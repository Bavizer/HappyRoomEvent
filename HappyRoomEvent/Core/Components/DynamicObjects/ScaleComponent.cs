using UnityEngine;

namespace HappyRoomEvent.Core.Components.DynamicObjects;

[DisallowMultipleComponent]
public class ScaleComponent : DynamicObjectComponent
{
    protected override void Update()
    {
        Duration -= Time.deltaTime;
        Vector3 scale = transform.localScale;
        transform.localScale = Vector3.MoveTowards(scale, scale + Axis, Speed * Time.deltaTime);
    }
}
