using EIV_Common.JsonStuff;
using EIV_JsonLib.Interfaces;

namespace ExtractIntoVoid.Items;

public abstract partial class ThrowableBase : UsableBase
{
    public override void OnUsingFinished()
    {
        var throwable = UsableItem.As<IThrowable>();
        base.OnUsingFinished();
        if (!throwable.HasValidAssetPath())
            return;

        // TODO: AssetPath has a bath to the Scene that has the throwable object.
    }
}
