using Godot;
using Godot.NativeInterop;

namespace ExtractIntoVoid.Menus;

public partial class MainUI : Control
{
    [Export]
    public Panel BasePanel;

    public override void _Ready()
    {
        BasePanel = this.GetNode<Panel>("BasePanel");
        SetShader(0, 0, 0, 0, 0);
    }

    public void SetShader(float OuterRadius, float MainAlpha, float Red, float Green, float Blue)
    {
        var shader = BasePanel.Material as ShaderMaterial;
        shader.SetShaderParameter("OuterRadius", OuterRadius);
        shader.SetShaderParameter("MainAlpha", MainAlpha);
        shader.SetShaderParameter("Red", Red);
        shader.SetShaderParameter("Green", Green);
        shader.SetShaderParameter("Blue", Blue);
    }
}
