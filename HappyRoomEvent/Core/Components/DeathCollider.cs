using LabApi.Features.Wrappers;
using UnityEngine;

namespace HappyRoomEvent.Core.Components;

[DisallowMultipleComponent]
public class DeathCollider : MonoBehaviour
{
#nullable disable
    public BoxCollider Collider { get; private set; }
#nullable restore

    public bool IsTrigger
    {
        get => Collider.isTrigger;
        set => Collider.isTrigger = value;
    }

    protected virtual void Awake()
    {
        Collider = gameObject.AddComponent<BoxCollider>();

        enabled = false;
    }

    protected virtual void OnCollisionStay(Collision collision)
    {
        if (enabled)
            KillIfPlayer(collision.gameObject);
    }

    protected virtual void OnTriggerStay(Collider collider)
    {
        if (enabled)
            KillIfPlayer(collider.gameObject);
    }

    private void KillIfPlayer(GameObject obj)
    {
        if (!obj.CompareTag("Player"))
            return;

        var player = Player.Get(obj);
        if (player?.IsAlive ?? false)
            player.Kill();
    }
}
