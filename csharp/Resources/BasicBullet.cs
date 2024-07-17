using Godot;

namespace ExtractIntoVoid.Resources;

public partial class BasicBullet : Resource
{
    [Export]
    public virtual Vector3 MeshScale { get; set; }

    [Export]
    public virtual Mesh Mesh { get; set; }

    [Export]
    public virtual float ShapeRadius { get; set; }

    [Export]
    public virtual float ShapeHeight { get; set; }

    [Export]
    public virtual Vector3 ShapePos { get; set; }
}