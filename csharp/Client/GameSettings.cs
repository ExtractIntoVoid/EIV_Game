using Godot;

namespace ExtractIntoVoid.Client;

public partial class GameSettings : Resource
{
    [Export]
    public bool Borderless = false;
    [Export]
    public bool Exclusive = false;
    [Export]
    public Window.ModeEnum Mode = Window.ModeEnum.Windowed;

    [Export]
    public Viewport.Msaa MSAA2D = Viewport.Msaa.Disabled;

    [Export]
    public Viewport.Msaa MSAA3D = Viewport.Msaa.Disabled;

    [Export]
    public Viewport.Scaling3DModeEnum Scaling3DMode = Viewport.Scaling3DModeEnum.Bilinear;

    [Export]
    public Scaling3DScaleEnum Scaling3DScale = Scaling3DScaleEnum.Balanced;

    [Export]
    public Viewport.ScreenSpaceAAEnum ScreenSpaceAA = Viewport.ScreenSpaceAAEnum.Disabled;

    [Export]
    public ScreenSizeEnum ScreenSize = ScreenSizeEnum._1280x720;
}

public enum Scaling3DScaleEnum : byte
{
    Performance,
    Balanced,
    Quality,
    UltraQuality
}

public enum ScreenSizeEnum : byte
{
    _3840x2160,
    _2560x1440,
    _1920x1080,
    _1280x720,
}
