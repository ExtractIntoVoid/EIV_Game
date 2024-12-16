using ExtractIntoVoid.Modules;
using Godot;

namespace ExtractIntoVoid.Physics;

public abstract partial class BasePlayer : CharacterBody3D
{
    public int NetId { get; set; }
    public string PlayerName { get; set; }
    public string UserId { get; set; }
    public string UserToken { get; set; }
    // Nodes connected to it.

    // add bones here, and maybe animations.
    public Node HandlePoint { get; set; }
    public Camera3D Camera { get; set; }

    public override void _EnterTree()
    {
        if (!int.TryParse(Name.ToString(), out int netId))
            return;
        NetId = netId;
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
        this.AddModuleNode(new HealthModule());
        this.AddModuleNode(new EnergyModule());
        this.AddModuleNode(new HydrationModule());
        this.AddModuleNode(new EffectModule());
        this.AddModuleNode(new InventoryModule());
        var health = this.GetModuleNode<HealthModule>();
        health.OnDeath += OnDeath;
    }

    public virtual void OnDeath(object sender, string e)
    {
        // TODO: show lobby instead of just quitting.
        Managers.GameManager.Instance.Quit();
    }
}
