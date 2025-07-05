using HappyRoomEvent.Extensions;
using HappyRoomEvent.Tools;
using ProjectMER.Features;
using ProjectMER.Features.Objects;
using System;
using UnityEngine;

namespace HappyRoomEvent.Core;

public readonly struct HappyRoom
{
    public readonly SchematicObject Schematic;

    public readonly Bounds Bounds;

    public readonly LightSourceToy RoomLight;

    public readonly Transform SpawnPoint;

    internal HappyRoom(Vector3 position, Quaternion rotation)
    {
        try
        {
            Schematic = ObjectSpawner.SpawnSchematic("HappyRoom", position, rotation);
            Transform room = Schematic.transform.Find("Room");

            foreach (var transform in room.GetComponentsInChildren<Transform>())
            {
                switch (transform.name)
                {
                    case "RoomBounds":
                        Bounds = new Bounds(transform.position, transform.localScale);
                        break;

                    case "RoomLight":
                        RoomLight = transform.GetComponent<LightSourceToy>();
                        break;

                    case "SpawnPoint":
                        SpawnPoint = transform;
                        break;
                }
            }

            Schematic.AttachedBlocks.ForEach(b => DynamicObjectNameParser.ParseFromName(b));
        }
        catch (Exception ex)
        {
            Logger.Error($"An exception occurred when creating Happy Room schematic: {ex}");
            throw;
        }
    }
}
