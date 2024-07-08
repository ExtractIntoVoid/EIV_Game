using EIV_JsonLib.Interfaces;
using ExtractIntoVoid.Modules;
using Godot;

namespace ExtractIntoVoid.Items
{
    public abstract partial class UsableBase : RigidBody3D
    {
        public string ApplyTo;
        public PlayerModule Modules;
        public IUsable UsableItem { get; set; }
        bool IsInUse = false;
        decimal UseTime = 0;

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

        }

        public virtual void OnUsingCancelled()
        {

        }

        public virtual void OnUsingFinished()
        {
            IsInUse = false;
            UsableItem.CanUse = false;
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
}
