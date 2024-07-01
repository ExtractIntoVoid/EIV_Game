#if SERVER || GAME
using EIV_Common;
using ExtractIntoVoid.Managers;

namespace ExtractIntoVoid.Server
{
    public class NetManager
    {
        public static int GetPort()
        {
            int port = ConfigINI.Read<int>(GameManager.INI, "Net", "Port");
            if (port == 0)
                return 7878;
            return port;
        }

        public static int GetMaxClients()
        {
            int maxClients = ConfigINI.Read<int>(GameManager.INI, "Net", "MaxClients");
            if (maxClients == 0)
                return 32;
            return maxClients;
        }

        public static string GetBindIP()
        {
            return ConfigINI.Read(GameManager.INI, "Net", "BindIP");
        }

        public static string GetExternalIP()
        {
            return ConfigINI.Read(GameManager.INI, "Net", "ExternalIP");
        }
    }
}
#endif