using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using System;

namespace HappyRoomEvent;

internal sealed class Plugin : Plugin<Config>
{
#nullable disable
    public static Plugin Instance { get; private set; }
#nullable restore

    public override string Name => "Happy Room Event";

    public override string Description => "A Game Mode in which different Sub-Events are played in Happy Room.";

    public override string Author => "Bavizer";

    public override Version Version => new(1, 0, 0, 0);

    public override Version RequiredApiVersion => new(LabApiProperties.CompiledVersion);

    public override void Enable() => Instance = this;

    public override void Disable()
    {
        var happyRoom = Core.HappyRoomEvent.Instance;

        if (happyRoom.IsActive)
            happyRoom.EndEvent();
    }

    public void LoadAndSaveConfig()
    {
        LoadConfigs();
        SaveConfig();
    }
}
