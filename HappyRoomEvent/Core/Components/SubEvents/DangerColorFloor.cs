using UnityEngine;

namespace HappyRoomEvent.Core.Components.SubEvents;

[DisallowMultipleComponent]
public class DangerColorFloor : MonoBehaviour
{
    #region Fields
#nullable disable

    private DeathCollider _deathCollider;
    private PrimitiveObjectToy _primitive;

    private float _elapsedTimeSwitching;

    public bool ShouldSwitchToDangerous;

#nullable restore
    #endregion

    #region Properties

    public Color DefaultColor
    {
        get => field == default ? _primitive.NetworkMaterialColor : field;
        set => field = _primitive.NetworkMaterialColor = value;
    }

    public Color DangerColor { get; set; } = Color.red;

    public float SwitchDuration
    {
        get; 
        set => field = value > 0 ? value : 0;
    } = 3f;

    public bool IsDangerous => _primitive.NetworkMaterialColor == DangerColor;

    #endregion

    #region Methods

    public void Init(Color defaultColor, Color dangerColor, float switchDuration)
    {
        DefaultColor = defaultColor;
        DangerColor = dangerColor;
        SwitchDuration = switchDuration;
    }

    public void Reset()
    {
        ShouldSwitchToDangerous = false;
        _elapsedTimeSwitching = 0f;
        _primitive.NetworkMaterialColor = DefaultColor;
    }

    protected virtual void Awake()
    {
        _deathCollider = gameObject.AddComponent<DeathCollider>();
        _primitive = GetComponent<PrimitiveObjectToy>();

        _deathCollider.Collider.center = new Vector3(0, 1, 0);
        _deathCollider.IsTrigger = true;

        enabled = false;
    }

    protected virtual void OnDisable() => _deathCollider.enabled = false;

    protected virtual void Update()
    {
        _deathCollider.enabled = IsDangerous;

        if (!ShouldSwitchToDangerous || IsDangerous)
            return;

        _elapsedTimeSwitching += Time.deltaTime;
        _primitive.NetworkMaterialColor = Color.Lerp(DefaultColor, DangerColor, _elapsedTimeSwitching / SwitchDuration);
    }

    #endregion
}
