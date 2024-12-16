using EIV_JsonLib.Base;
using Godot;

namespace ExtractIntoVoid.Items;

public abstract partial class UsableBase : InventoryItemBase
{
    AnimationPlayer Animation;
    public CoreUsable UsableItem { get; set; }
    bool IsInUse = false;
    decimal UseTime = 0;

    public UsableBase() : base()
    {
        if (HasNode("Animation"))
            Animation = GetNode<AnimationPlayer>("Animation");
        Animation = null;
    }

    public virtual void UsingStart()
    {
        UseTime = UsableItem.UseTime;
        UsableItem.CanUse = false;
        IsInUse = true;
    }

    public virtual void UsingCancel()
    {
        if (IsInUse)
        {
            UsableItem.CanUse = true;
            IsInUse = false;
            UseTime = UsableItem.UseTime;
        }
    }
    public virtual void OnUsingStarted()
    {
        if (Animation != null)
            Animation.Play("Start");
    }

    public virtual void OnUsingCancelled()
    {
        if (Animation != null)
            Animation.Play("RESET");
    }

    public virtual void OnUsingFinished()
    {
        IsInUse = false;
        UsableItem.CanUse = false;
        InventoryModule.FinishedUsing();
    }

    public override void _Process(double delta)
    {
        if (IsInUse)
        {
            if (UsableItem.UseTime == UseTime)
            {
                OnUsingStarted();
            }
            UseTime--;
            if (UseTime == 0)
            {
                OnUsingFinished();
            }
            OnTick(delta);
        }
    }

    public virtual void OnTick(double delta)
    {

    }
}
