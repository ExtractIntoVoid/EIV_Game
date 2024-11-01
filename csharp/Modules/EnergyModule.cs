using System;

namespace ExtractIntoVoid.Modules;

public partial class EnergyModule : BaseChangingModule<int>
{
    public EnergyModule() : base(0, 100)
    {

    }

}
