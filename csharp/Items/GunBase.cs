using EIV_Common.Extensions;
using EIV_Common.JsonStuff;
using EIV_JsonLib;
using ExtractIntoVoid.Physics;
using Godot;
using MemoryPack;
using System.Collections.Generic;
using System.Linq;

namespace ExtractIntoVoid.Items;

public abstract partial class GunBase : InventoryItemBase
{
    Marker3D BulletSpawner;
    public virtual Gun Gun { get; set; }

    public bool IsJammed { get; internal set; } = false;
    public override void _Ready()
    {
        BulletSpawner = GetNode<Marker3D>("BulletSpawner");
    }

    public virtual void Shoot()
    {
        if (Gun.Magazine == null)
            return;
        if (IsJammed)
            return;
        if (Gun.Magazine.Ammunitions.Count > 0)
        {
            var ammo = Gun.Magazine.Ammunitions.First();
            // Ammo doesnt have 3D representation of itself :(
            if (!ammo.HasValidAssetPath())
            {
                PlayJammed();
                return;
            }
            var id = GD.Randi();
            var BulletSpawnTransform = BulletSpawner.GlobalTransform.LookingAt(BulletSpawner.Position);
            Rpc("ShootBullet", (int)id, MemoryPackSerializer.Serialize(ammo), BulletSpawnTransform);
            GetNode<AnimationPlayer>("AnimationPlayer").Play("Shoot");
            // UI update here
            Gun.Magazine.Ammunitions.Remove(ammo);
        }
    }

    public virtual void PlayJammed()
    {
        GD.Print("Your gun is jammed. please \"reload\" to eject current faulty ammo");
        IsJammed = true;
    }

    public virtual void Reload()
    {
        // TMP.
        IsJammed = false;
        // todo reload. mainly eject the mag. and if another mag exist in inventory use that.
        // eject mag should be 3d object or should be sent back?
        // first in if its compatible with the gun.
    }

    // RPC
    [Rpc(mode: MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    private void ShootBullet(int id, byte[] ammo, Transform3D transform)
    {
        PackedScene bulletScene = null;
        var iammo = MemoryPackSerializer.Deserialize<Ammo>(ammo);
        bulletScene = ResourceLoader.Load<PackedScene>(iammo.AssetPath);
        if (!iammo.HasValidAssetPath())
        {
            GD.Print("Bullet Couldnt spawned because AssetPath is not valid!");
            return;
        }
        if (bulletScene == null)
        {
            GD.Print("Bullet Couldnt spawned because the scene is null");
            return;
        }
  
        //
        var bullet = bulletScene.Instantiate<Bullet>();
        bullet.Name = "Bullet_" + id.ToString();
        bullet.SetMP(id);
        bullet.SetAmmo(iammo);
        GetTree().GetNodesInGroup("Worlds")[0].AddChild(bullet);
        bullet.GlobalTransform = transform;
    }
}
