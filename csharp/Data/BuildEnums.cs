namespace ExtractIntoVoid;

public enum BuildType : byte
{
    None,
    Game,
    Server,
    Client,
}

public enum ReleaseType : byte
{
    Production,
    Testing,
    Development
}
