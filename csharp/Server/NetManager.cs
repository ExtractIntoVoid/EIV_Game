#if SERVER || GAME
using EIV_Common;
using ExtractIntoVoid.Managers;
using System.Linq;

namespace ExtractIntoVoid.Server
{
    public class NetManager
    {
        public static int GetPort()
        {
            int port = 0;
            // TODO: move this out and we could get others of this.
            var cmd_args = Godot.OS.GetCmdlineArgs();
            if (cmd_args.Contains("--port="))
            {
                foreach (var item in cmd_args)
                {
                    if (item.Contains("port"))
                    {
                        var string_port = item.Split("=")[1];
                        if (int.TryParse(string_port, out port))
                            return port;
                    }
                }
            }
            port = ConfigINI.Read<int>(BuildDefined.INI, "Net", "Port");
            if (port == 0)
                return 7878;
            return port;
        }

        public static string GetBindIP()
        {
            return ConfigINI.Read(BuildDefined.INI, "Net", "BindIP");
        }

        public static string GetExternalIP()
        {
            return ConfigINI.Read(BuildDefined.INI, "Net", "ExternalIP");
        }
    }
}
#endif