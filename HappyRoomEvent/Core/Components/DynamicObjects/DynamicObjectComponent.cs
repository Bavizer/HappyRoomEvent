using UnityEngine;

namespace HappyRoomEvent.Core.Components.DynamicObjects;

public abstract class DynamicObjectComponent : MonoBehaviour
{
    public float Speed;

    public Vector3 Axis;

    public float Duration 
    { 
        get; 
        set => field = value > 0 ? value : 0;
    }

    public void Init(float duration, Vector3 delta)
    {
        Speed = delta.magnitude;
        Duration = duration;
        Axis = delta;
    }

    protected abstract void Update();
}
