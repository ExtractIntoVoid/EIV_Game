using EIV_JsonLib.Interfaces;
using ExtractIntoVoid.Items;
using ExtractIntoVoid.Managers;
using ExtractIntoVoid.Physics;
using Godot;
using System.Linq;

namespace ExtractIntoVoid.Modules;

public partial class InventoryNodeModule : Node, IModule
{
    public BasePlayer CurrentPlayer;

    public RigidBody3D CurrentItemNode;

    public void SelectItem(IItem selectedItem)
    {
        if (selectedItem == null)
            return;

        CurrentItemNode = null;

        if (GameManager.Instance.SceneManager.Scenes.ContainsKey(selectedItem.BaseID))
        {
            GD.Print("Item does not have a scene inside SceneManager!.");
            Rpc("ChangeHandScene", CurrentPlayer.NetId, string.Empty);
            return;
        }
        Rpc("ChangeHandScene", CurrentPlayer.NetId, selectedItem.BaseID);
    }

    public void Usable_Use()
    {
        var usableBase = CurrentItemNode as UsableBase;
        usableBase.UsingStart();
    }

    public void Gun_Use()
    {
        var gun = CurrentItemNode as GunBase;
        gun.SetMP(CurrentPlayer.NetId);
        gun.Shoot();
    }


    #region RPC
    [Rpc(mode: MultiplayerApi.RpcMode.Authority, CallLocal = false, TransferChannel = 2, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void ChangeHandScene(int id, string scene)
    {
        GD.Print("ChangeHandScene!. " + id + " " + scene);
        var playerNode = this.GetTree().GetNodesInGroup("Player").Where(x => x.Name == id.ToString()).FirstOrDefault();
        if (playerNode == null)
        {
            GD.Print("playerNode null!.");
            return;
        }
        var player = playerNode as BasePlayer;
        if (scene == string.Empty)
        {
            GD.Print("freeing HandlePoint!.");
            Rpc("Client_ClearHandScene", id, playerNode.GetPath());
            foreach (var item in player.HandlePoint.GetChildren())
            {
                item.QueueFree();
            }
            return;
        }
        var node = GameManager.Instance.SceneManager.GetPackedScene(scene).Instantiate<RigidBody3D>();
        node.SetMultiplayerAuthority(id);
        player.HandlePoint.AddChild(node);
        GD.Print("successfull!.");
        CurrentItemNode = node;
        Rpc("Client_ChangeHandScene", id, scene, playerNode.GetPath());
    }

    [Rpc(mode: MultiplayerApi.RpcMode.AnyPeer, CallLocal = false, TransferChannel = 2, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void Client_ChangeHandScene(int id, string scene, string playernode)
    {
        GD.Print("Client_ChangeHandScene!. " + id + " " + scene);
        var player = this.GetNode(playernode) as BasePlayer;
        var node = GameManager.Instance.SceneManager.GetPackedScene(scene).Instantiate<RigidBody3D>();
        node.SetMultiplayerAuthority(id);
        player.HandlePoint.AddChild(node);
        if (player.NetId == id)
            CurrentItemNode = node;
    }

    [Rpc(mode: MultiplayerApi.RpcMode.AnyPeer, CallLocal = false, TransferChannel = 2, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void Client_ClearHandScene(int id, string playernode)
    {
        GD.Print("Client_ClearHandScene!. " + id + " " + playernode);
        var player = this.GetNode(playernode) as BasePlayer;
        foreach (var item in player.HandlePoint.GetChildren())
        {
            item.QueueFree();
        }
        if (player.NetId == id)
        {
            CurrentItemNode.QueueFree();
            CurrentItemNode = null;
        }
    }
    #endregion
}
