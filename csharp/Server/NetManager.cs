#if SERVER || GAME
using EIV_Common;
using ExtractIntoVoid.Managers;

namespace ExtractIntoVoid.Server
{
    public class NetManager
    {
        public static int GetPort()
        {
            int port = 0;
            if (ArgManager.HasArgParam("port"))
            {
                if (int.TryParse(ArgManager.GetArgParam("port"), out port))
                    return port;
            }
            port = ConfigINI.Read<int>(BuildDefined.INI, "Net", "Port");
            if (port == 0)
                return 7878;
            return port;
        }

        public static string GetBindIP()
        {
            if (ArgManager.HasArgParam("bindip"))
                return ArgManager.GetArgParam("bindip");
            return ConfigINI.Read(BuildDefined.INI, "Net", "BindIP");
        }

        public static string GetExternalIP()
        {
            if (ArgManager.HasArgParam("extip"))
                return ArgManager.GetArgParam("extip");
            return ConfigINI.Read(BuildDefined.INI, "Net", "ExternalIP");
        }
    }
}
#endif