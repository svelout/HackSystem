using System.Linq;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using ServerOutput;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace HackSystem
{
    public class Plugin : Plugin<Config>
    {
        public override string Author => "SveloutDevelops";
        
        public override string Name => "HackSystem";
        
        public override string Prefix => "hack_system";

        public static Plugin Instance;
        private EventHandlers eh;

        public override void OnEnabled()
        {
            Instance = this;
            eh = new();
            Server.RoundStarted += eh.OnRoundStarted;
            Player.InteractingDoor += eh.OnInteractingDoors;
            
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Instance = null;
            
            Server.RoundStarted -= eh.OnRoundStarted;
            Player.InteractingDoor -= eh.OnInteractingDoors;
            eh = null;
            
            base.OnDisabled();
        }

        internal static void RunRnrCommand()
        {
            if (ServerStatic.StopNextRound == ServerStatic.NextRoundAction.Restart)
            {
                ServerLogs.AddLog(ServerLogs.Modules.Administrative, "" + " canceled server restart after the round end.", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
                ServerStatic.StopNextRound = ServerStatic.NextRoundAction.DoNothing;
                ServerConsole.AddOutputEntry(default(ExitActionResetEntry));
                Log.Debug("Server WON'T restart after next round.");
            }
            else
            {
                ServerStatic.StopNextRound = ServerStatic.NextRoundAction.Restart;
                ServerConsole.AddOutputEntry(default(ExitActionRestartEntry));
                Log.Debug("Server WILL restart after next round.");
            }
        }
    }
}