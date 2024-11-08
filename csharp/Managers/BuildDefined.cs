using Godot;
using System;
using System.IO;

namespace ExtractIntoVoid.Managers;

public static class BuildDefined
{

    public static SemanticVersioning.Version Version = new("0.0.1-alpha");

    public static string FullVersion = $"EIV {Build} {Release} {Version.Clean()}";

    public static byte[] EIV_SaveKey = Convert.FromHexString("c4ecd705c59f666bdfc3281a2295f337d0161ad2b3e727e5a71b0710c80424df");
    public static BuildType Build
    {
        get =>
#if GAME
            BuildType.Game;
#elif CLIENT
            BuildType.Client;
#elif SERVER
            BuildType.Server;
#else
            BuildType.None;
#endif
    }

    public static ReleaseType Release
    {
        get =>
#if DEBUG && TESTING
            ReleaseType.Testing;
#elif DEBUG
            ReleaseType.Development;
#else
            ReleaseType.Production;
#endif
    }


    public static string INI = Path.Combine(Path.GetDirectoryName(OS.GetExecutablePath()),
#if GAME
        "Game.ini"
#elif SERVER
        "Server.ini"
#elif CLIENT
        "Client.ini"
#endif
        );
}
