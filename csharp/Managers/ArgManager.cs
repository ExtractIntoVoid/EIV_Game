using System;
using System.Linq;

namespace ExtractIntoVoid.Managers;

public static class ArgManager
{
    public static string GetArgParam(string search)
    {
        var cmd_args = Godot.OS.GetCmdlineArgs().AsSpan();
        if (cmd_args.Contains($"--{search}="))
        {
            foreach (var item in cmd_args)
            {
                if (item.Contains(search))
                {
                    var string_port = item.Split("=")[1];
                    return string_port;
                }
            }
        }
        cmd_args.Clear();
        return string.Empty;
    }

    public static bool HasArgParam(string search)
    {
        var cmd_args = Godot.OS.GetCmdlineArgs().AsSpan();
        return cmd_args.Contains($"--{search}=");
    }

    public static bool HasArg(string search)
    {
        var cmd_args = Godot.OS.GetCmdlineArgs().AsSpan();
        return cmd_args.Contains($"--{search}");
    }
}
