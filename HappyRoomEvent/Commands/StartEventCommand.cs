using CommandSystem;
using System;

namespace HappyRoomEvent.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class StartEventCommand : ICommand
{
    public string Command => "start_hroom";

    public string[] Aliases => [];

    public string Description => "Start Happy Room Event";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission(PlayerPermissions.RoundEvents, out response))
            return false;

        try
        {
            Core.HappyRoomEvent.Instance.StartEvent();
        }
        catch (InvalidOperationException ex)
        {
            response = ex.Message;
            return false;
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            response = "An error occured when starting event.";
            return false;
        }

        Logger.Info($"{sender.LogName} started the event.");
        response = "Event has been started.";
        return true;
    }
}
