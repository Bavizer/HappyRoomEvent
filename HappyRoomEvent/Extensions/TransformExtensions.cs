using System.Linq;
using UnityEngine;

namespace HappyRoomEvent.Extensions;

public static class TransformExtensions
{
    public static PrimitiveObjectToy[] GetNonDefaultPrimitivesInChildren(this Transform transform)
    {
        return transform.GetComponentsInChildren<PrimitiveObjectToy>(true)
            .Where(p => !p.IsDefaultPrefab()).ToArray();
    }
}

