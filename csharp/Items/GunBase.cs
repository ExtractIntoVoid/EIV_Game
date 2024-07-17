using EIV_Common.JsonStuff;
using EIV_JsonLib.Convert;
using EIV_JsonLib.Interfaces;
using ExtractIntoVoid.Managers;
using ExtractIntoVoid.Modules;
using ExtractIntoVoid.Physics;
using ExtractIntoVoid.Resources;
using Godot;
using System.Collections.Generic;
using System.Linq;

namespace ExtractIntoVoid.Items
{
    public abstract partial class GunBase : RigidBody3D
    {
        Marker3D BulletSpawner;
        public virtual IGun Gun { get; set; }
        public virtual List<IAmmo> Ammos { get; set; } = [];
        public PlayerModule Modules;

        public bool IsJammed { get; internal set; } = false;
        public override void _Ready()
        {
            BulletSpawner = GetNode<Marker3D>("BulletSpawner");
            if (Gun.Magazine != null)
            {
                foreach (var item in Gun.Magazine.Ammunition)
                {
                    var ammo = ItemMaker.CreateItem<IAmmo>(item);
                    if (ammo == null)
                        continue;
                    Ammos.Add(ammo);
                }
            }
        }

        public void SetMP(int NetId)
        {
            SetMultiplayerAuthority(NetId);
            GetNode<MultiplayerSynchronizer>("MSync").SetMultiplayerAuthority(NetId);
        }

        public virtual void Shoot()
        {
            if (Gun.Magazine == null)
                return;
            if (IsJammed)
                return;
            if (Ammos.Count > 0)
            {
                var ammo = Ammos.First();
                // Ammo doesnt have 3D representation of itself :(
                if (!ammo.HasValidAssetPath())
                {
                    PlayJammed();
                    return;
                }
                var id = GD.Randi();
                var BulletSpawnTransform = BulletSpawner.GlobalTransform;
                BulletSpawnTransform = BulletSpawnTransform.LookingAt(BulletSpawner.Position);
                Rpc("ShootBullet", (int)id, MPConvertHelper.Serialize(ammo), BulletSpawnTransform);
                GetNode<AnimationPlayer>("AnimationPlayer").Play("Shoot");
                // UI update here
                Ammos.Remove(ammo);
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
            var iammo = MPConvertHelper.Deserialize<IAmmo>(ammo);
            bulletScene = GameManager.Instance.SceneManager.GetPackedScene("AmmoBasic");
            if (bulletScene == null)
            {
                GD.Print("Bullet Couldnt spawned because BulletScene is null");
                return;
            }
            if (!iammo.HasValidAssetPath())
            {
                GD.Print("Bullet Couldnt spawned because AssetPath is not valid!");
                return;
            }
            //
            var bullet = bulletScene.Instantiate<Bullet>();
            var bullet_res = ResourceLoader.Load<BasicBullet>(iammo.AssetPath);
            GD.Print(bullet_res.Mesh.ResourcePath);
            bullet.Name = "Bullet_" + id.ToString();
            var mesh = bullet.GetChild<MeshInstance3D>(0);
            mesh.Mesh = bullet_res.Mesh;
            mesh.Scale = bullet_res.MeshScale;
            var collision = bullet.GetChild<CollisionShape3D>(1);
            collision.Position = bullet_res.ShapePos;
            var capsule = collision.Shape as CapsuleShape3D;
            capsule.Radius = bullet_res.ShapeRadius;
            capsule.Height = bullet_res.ShapeHeight;
            bullet.SetMultiplayerAuthority(id);
            bullet.SetAmmo(iammo);
            GetTree().GetNodesInGroup("Worlds")[0].AddChild(bullet);
            //GetTree().Root.AddChild(bullet);
            bullet.GlobalTransform = transform;
            //bullet.GlobalRotation = lookat;
        }
    }
}
