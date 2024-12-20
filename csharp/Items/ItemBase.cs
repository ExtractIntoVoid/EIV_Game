﻿using ExtractIntoVoid.Modules;
using ExtractIntoVoid.Physics;
using Godot;

namespace ExtractIntoVoid.Items;

public abstract partial class InventoryItemBase : RigidBody3D
{
    public InventoryModule InventoryModule { get; set; }
    public BasePlayer Player { get; set; }
}
