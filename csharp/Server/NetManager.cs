#if SERVER
using ExtractIntoVoid.Controllers;
using System;

namespace ExtractIntoVoid.Server
{
    public static class NetManager
    {
        public static int GetPort()
        {
            string port = INIController.Read("Server.ini", "Net", "Port");
            int result = 0;
            bool ret = int.TryParse(port, out result);
            if (ret)
                Console.WriteLine("Server.ini: Net.Port cannot parse to int!");
            return result;
        }

        public static int GetMaxClients()
        {
            string port = INIController.Read("Server.ini", "Net", "MaxClients");
            int result = 32;
            bool ret = int.TryParse(port, out result);
            if (ret)
                Console.WriteLine("Server.ini: Net.MaxClients cannot parse to int!");
            return result;
        }

        public static string GetBindIP()
        {
            return INIController.Read("Server.ini", "Net", "BindIP");
        }

        public static string GetExternalIP()
        {
            return INIController.Read("Server.ini", "Net", "ExternalIP");
        }
    }
}
#endif