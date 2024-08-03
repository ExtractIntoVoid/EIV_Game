using ExtractIntoVoid.Modules;
using Godot;

namespace ExtractIntoVoid.Physics;

public abstract partial class BasePlayer : CharacterBody3D
{
    public int NetId { get; set; }

    public string PlayerName { get; set; }
    // Nodes connected to it.

    // add bones here, and maybe animations.
    public Node HandlePoint { get; set; }
    public Camera3D Camera { get; set; }

    // Modules and others
    public PlayerModule PlayerModule { get; set; }

    public override void _EnterTree()
    {
        NetId = Name.ToString().ToInt();
        SetMultiplayerAuthority(NetId);
        GetNode<MultiplayerSynchronizer>("MSync").SetMultiplayerAuthority(NetId);
    }

    public override void _Ready()
    {
        Camera = GetNode<Camera3D>("Camera");
        HandlePoint = GetNode("HandlePoint");
        if (IsMultiplayerAuthority())
        {
            Camera.Current = true;
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }
        PlayerModule = new(this);
    }
}
