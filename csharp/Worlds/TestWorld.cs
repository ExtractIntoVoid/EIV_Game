
namespace ExtractIntoVoid.Worlds;

public partial class TestWorld : BasicWorld
{
    public override string MapName => "TestWorld";

    public override int MinPlayerCount => 2;

    public override int MaxPlayerCount => 32;

    public override void _Ready()
    {
        base._Ready();
    }
}
