#if SERVER || GAME
using ExtractIntoVoid.Controllers;
using ExtractIntoVoid.Managers;
using System;

namespace ExtractIntoVoid.Server
{
    public class NetManager
    {
        public static int GetPort()
        {
            string port = INIController.Read(GameManager.INI, "Net", "Port");
            int result = 7878;
            bool ret = int.TryParse(port, out result);
            if (!ret)
                Console.WriteLine("Server.NetManager: Net.Port cannot parse to int!");
            return result;
        }

        public static int GetMaxClients()
        {
            string port = INIController.Read(GameManager.INI, "Net", "MaxClients");
            int result = 32;
            bool ret = int.TryParse(port, out result);
            if (!ret)
                Console.WriteLine("Server.NetManager: Net.MaxClients cannot parse to int!");
            return result;
        }

        public static string GetBindIP()
        {
            return INIController.Read(GameManager.INI, "Net", "BindIP");
        }

        public static string GetExternalIP()
        {
            return INIController.Read(GameManager.INI, "Net", "ExternalIP");
        }
    }
}
#endif