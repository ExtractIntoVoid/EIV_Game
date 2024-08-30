using System;

namespace ExtractIntoVoid;

[Flags]
public enum Permissions : ulong
{
    None = 0,
    Kick = 1,
    Ban = 2,
    InventoryManagement = 4,
    EffectManagament = 8,
    PlayerManagement = 16,
    ModManagement = 32,
    RoundManagement = 64,

}
