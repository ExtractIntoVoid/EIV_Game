using IniParser;
using IniParser.Model;

namespace ExtractIntoVoid.Controllers
{
    public class INIController
    {
        public static string Read(string name, string section)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Config.ini", System.Text.Encoding.UTF8);
            return data[name][section];
        }

        public static void Write(string name, string section, string Value)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Config.ini", System.Text.Encoding.UTF8);
            data[name][section] = Value;
            parser.WriteFile("Config.ini", data, System.Text.Encoding.UTF8);
        }

    }
}
