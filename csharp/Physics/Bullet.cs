using EIV_JsonLib.Interfaces;
using ExtractIntoVoid.Modules;
using Godot;

namespace ExtractIntoVoid.Physics;

public partial class Bullet : Area3D
{
    [Export]
    public float Speed = 1f;

    [Export]
    public float TimeToLive = 5f;

    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    private IAmmo Ammo;

    public override void _EnterTree()
    {
        GetNode<MultiplayerSynchronizer>("MSync").SetMultiplayerAuthority(GetMultiplayerAuthority());
        this.BodyEntered += OnBodyEntered;
    }

    public override void _Process(double delta)
    {
        // Calculate to dont go to direction always, use the weight to be down a little i guess.
        Vector3 Pos = Position;
        Pos = Pos + -GlobalTransform.Basis.Z * Speed;
        Pos.Y = Pos.Y - gravity * (float)delta * 0.01f;
        Position = Pos;

        TimeToLive -= (float)delta;
        if (TimeToLive <= 0)
            BulletDestroyed();
    }

    public virtual void SetAmmo(IAmmo ammo)
    {
        Ammo = ammo;
        //Speed = Ammo.Speed;
    }

    public void OnBodyEntered(Node3D Body)
    {
        if (Body.TryGetModuleNode(out HealthModule healthModule))
        {
            healthModule.Damage((int)Ammo.Damage, Ammo.DamageType);
        }
        // add bullet hole?
        BulletDestroyed();
    }

    public virtual void BulletDestroyed()
    {
        this.BodyEntered -= OnBodyEntered;
        QueueFree();
    }
}
