using AdminToys;
using HappyRoomEvent.Core.Components.DynamicObjects;
using UnityEngine;

namespace HappyRoomEvent.Core.Components.SubEvents;

[DisallowMultipleComponent]
public class DeathRotor : MonoBehaviour
{
    #region Fields
#nullable disable

    private DeathCollider _deathCollider;
    private RotationComponent _rotationComponent;
    private PrimitiveObjectToy _primitive;

#nullable restore
    #endregion

    #region Properties

    public float AngularSpeed
    {
        get => _rotationComponent.Speed;
        set => _rotationComponent.Speed = value;
    }

    public float MaxAngularSpeed { get; set; }

    public float Acceleration { get; set; }

    public PrimitiveFlags PrimitiveFlags
    {
        get => _primitive.NetworkPrimitiveFlags;
        set => _primitive.NetworkPrimitiveFlags = value;
    }

    #endregion

    #region Methods

    public void Init(float angularSpeed, float maxAngularSpeed, float acceleration)
    {
        AngularSpeed = angularSpeed;
        MaxAngularSpeed = maxAngularSpeed;
        Acceleration = acceleration;
    }

    public void Reset()
    {
        AngularSpeed = 0f;
        transform.rotation = Quaternion.identity;
    }

    protected virtual void Awake()
    {
        _deathCollider = gameObject.AddComponent<DeathCollider>();
        _rotationComponent = gameObject.AddComponent<RotationComponent>();

        _deathCollider.IsTrigger = true;
        _rotationComponent.Init(float.PositiveInfinity, Vector3.up);

        _primitive = GetComponent<PrimitiveObjectToy>();

        enabled = false;
    }

    protected virtual void OnEnable()
    {
        _deathCollider.enabled = true;
        _rotationComponent.enabled = true;
        PrimitiveFlags = PrimitiveFlags.Visible;
    }

    protected virtual void OnDisable()
    {
        _deathCollider.enabled = false;
        _rotationComponent.enabled = false;
        PrimitiveFlags = PrimitiveFlags.None;
    }

    protected virtual void Update()
    {
        float deltaAcceleration = Acceleration * Time.deltaTime;
        float expectedSpeed = AngularSpeed + deltaAcceleration;
        AngularSpeed = expectedSpeed < MaxAngularSpeed ? expectedSpeed : MaxAngularSpeed;
    }

    #endregion
}
