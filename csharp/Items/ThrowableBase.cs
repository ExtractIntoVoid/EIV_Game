using EIV_Common.JsonStuff;
using EIV_JsonLib;

namespace ExtractIntoVoid.Items;

public abstract partial class ThrowableBase : UsableBase
{
    public override void OnUsingFinished()
    {
        var throwable = UsableItem.As<Throwable>();
        base.OnUsingFinished();
        if (!throwable.HasValidAssetPath())
            return;

        // TODO: AssetPath has a bath to the Scene that has the throwable object.
    }
}
