using AdminToys;
using HappyRoomEvent.Core.Components.DynamicObjects;
using UnityEngine;

namespace HappyRoomEvent.Core.Components.SubEvents;

[DisallowMultipleComponent]
public class Lava : MonoBehaviour
{
    #region Fields
#nullable disable

    private DeathCollider _deathCollider;
    private ScaleComponent _scaleComponent;
    private PrimitiveObjectToy _primitive;

    private Vector3 _initialScale;

#nullable restore
    #endregion

    #region Properties

    public Vector3 TargetScale
    {
        get;
        set
        {
            field = value;
            UpdateLavaScaleComponent();
        }
    }

    public float Duration
    {
        get;
        set
        {
            field = value > 0 ? value : 0;
            UpdateLavaScaleComponent();
        }
    }

    public PrimitiveFlags PrimitiveFlags
    {
        get => _primitive.NetworkPrimitiveFlags;
        set => _primitive.NetworkPrimitiveFlags = value;
    }

    #endregion

    #region Methods

    public void Init(Vector3 targetScale, float duration)
    {
        TargetScale = targetScale;
        Duration = duration;
    }

    public void Reset() => transform.localScale = _initialScale;

    protected virtual void Awake()
    {
        _deathCollider = gameObject.AddComponent<DeathCollider>();
        _scaleComponent = gameObject.AddComponent<ScaleComponent>();
        _primitive = GetComponent<PrimitiveObjectToy>();

        _deathCollider.IsTrigger = true;
        _initialScale = transform.localScale;

        enabled = false;
    }

    protected virtual void OnEnable()
    {
        _deathCollider.enabled = true;
        _scaleComponent.enabled = true;
        PrimitiveFlags = PrimitiveFlags.Visible;

        UpdateLavaScaleComponent();
    }

    protected virtual void OnDisable()
    {
        _deathCollider.enabled = false;
        _scaleComponent.enabled = false;
        PrimitiveFlags = PrimitiveFlags.None;
    }

    private void UpdateLavaScaleComponent()
    {
        Vector3 delta = TargetScale - transform.localScale;
        Vector3 speed = delta / Duration;
        _scaleComponent.Init(Duration, speed);
    }

    #endregion
}
