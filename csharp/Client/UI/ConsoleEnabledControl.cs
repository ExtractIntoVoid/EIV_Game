using System.Linq;
using ExtractIntoVoid.Managers;
using Godot;

public partial class ConsoleEnabledScene : Control {
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event is InputEventKey eKey && eKey.KeyLabel == Key.Quoteleft && eKey.IsReleased())
        {
            var consoles = GetChildren().OfType<DevConsole>();
            if (consoles.Any())
            {
                var first = consoles.First();

                first.Close();
            }
            else
            {
                AddChild(ConsoleManager.GetConsole());
            }
        }
    }
}