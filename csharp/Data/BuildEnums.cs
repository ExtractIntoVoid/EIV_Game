namespace ExtractIntoVoid;

public enum BuildType : byte
{
    None,
    Game,
    Server,
    Client,
}

public enum VersionType : byte
{
    Release,
    Testing,
    Development
}