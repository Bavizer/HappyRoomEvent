using ProjectMER.Features;

namespace HappyRoomEvent.Extensions
{
    public static class PrimitiveObjectExtensions
    {
        public static bool IsDefaultPrefab(this PrimitiveObjectToy toy)
        {
            var prefab = PrefabManager.PrimitiveObjectPrefab;
            return toy.MaterialColor == prefab.MaterialColor && toy.PrimitiveType == prefab.PrimitiveType;
        }
    }
}
