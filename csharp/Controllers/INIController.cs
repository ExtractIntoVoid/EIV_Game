using IniParser;
using IniParser.Model;
using System;

namespace ExtractIntoVoid.Controllers
{
    public class INIController
    {
        public static string Read(string filename, string section, string key)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(filename, System.Text.Encoding.UTF8);
            if (!data.Sections.ContainsSection(section))
                return string.Empty;
            if (!data[section].ContainsKey(key))
                return string.Empty;
            return data[section][key];
        }

        public static void Write(string filename, string category, string section, string Value)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(filename, System.Text.Encoding.UTF8);
            data[category][section] = Value;
            parser.WriteFile(filename, data, System.Text.Encoding.UTF8);
        }

    }
}
