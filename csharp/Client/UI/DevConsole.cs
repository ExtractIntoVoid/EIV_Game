using Godot;
using System;
using System.Linq;
using ExtractIntoVoid.Managers;
using System.Globalization;

// ReSharper disable CheckNamespace
// ReSharper disable MemberCanBePrivate.Global
public partial class DevConsole : Window
{
	// Called when the node enters the scene tree for the first time.

	private TextEdit Output => GetNode<TextEdit>("./Control/Output");
	private LineEdit Input => GetNode<LineEdit>("Control/InputArea/InputField");
    
    public override void _Ready()
    {
		CloseRequested += () =>
		{
			Close();
		};
		Open();
        //ConsoleManager.Console = this;
    }
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Visible && Output.Text != ConsoleManager.Contents)
			Output.Text = ConsoleManager.Contents;
	}
	private void  OnInputSubmitted(string input)
	{
		
		
		
		var command = input.Split(' ').First();
		var args = input.Split(' ').Skip(1).ToArray();//TakeLast(input.Split(' ').Length - 1).ToArray();
		Input.Text = "";
		Print("> " + input);
		
		HandleCommand(command, args);

	}
	public override void _Input(InputEvent @event)
	{
		if(@event is InputEventKey eventKey)
			if (eventKey.Pressed && eventKey.Keycode == Key.Tab && Input.Text != "")
			{
				var types = ConsoleManager.GetAll();
				if (!types.Keys.Any(s => s.StartsWith(Input.Text)))
				{
					return;
				}
				//Print(_cTypes.Keys.FirstOrDefault(s => s.StartsWith(Input.Text)));
				var suggestions = new PopupMenu()
				{
					Size = new Vector2I(0, 0),
					Position = new Vector2I((int)Input.GetScreenPosition().X, (int)Input.GetScreenPosition().Y + 24)
				};
				foreach (var item in types.Where(s => s.Key.StartsWith(Input.Text)))
				{
					var context = new CommandContext(this, null);

					var value = "";
					if (item.Value is CVar<string> cVarString)
					{
						value += " " + cVarString.Get(context);
					}
					if (item.Value is CVar<bool> cVarBool)
					{
						value += " " + cVarBool.Get(context);
					}
					if (item.Value is CVar<int> cVarInt)
					{
						value += " " + cVarInt.Get(context);
					}
					if (item.Value is CVar<float> cVarFloat)
					{
						value += " " + cVarFloat.Get(context);
					}
					suggestions.AddItem($"{item.Key}{value}");
				}
				suggestions.IdPressed += delegate(long id)
				{
					Input.Text = suggestions.GetItemText((int)id).Split(" ").First();
					Input.CaretColumn = Input.Text.Length;
				};
				suggestions.SetFocusedItem(0);
				Input.AddChild(suggestions);
				suggestions.Popup();
				//_cTypes.Keys.Where(s => s.StartsWith(Input.Text));
			}
		base._Input(@event);
	}

	public void Close()
	{
		Visible = false;
		Godot.Input.MouseMode = ConsoleManager.CachedMouse;
		QueueFree();
	}

	private void Open()
	{
        ConsoleManager.CachedMouse = Godot.Input.MouseMode;
		Visible = true;
		Godot.Input.MouseMode = Godot.Input.MouseModeEnum.Visible;
	}
	public void HandleCommand(string inputCommand, string[] args)
	{
		if (ConsoleManager.TryGet(inputCommand, out var command))
		{
			var context = new CommandContext(this, args);
			if (command is CFunc cFunc)
			{
				cFunc.Call(context);
			}
			if (command is CVar<string> cVarString)
			{
				if (args.Length == 0)
				{
					Print(cVarString.Get(context));
				}else {
					cVarString.Set(context, string.Join(" ", args));
				}
			}
			if (command is CVar<bool> cVarBool)
			{
				if (args.Length == 0)
				{
					Print(cVarBool.Get(context));
				}else {
					try {
						cVarBool.Set(context, Convert.ToBoolean(args[0]));

   					}
   					catch (FormatException) {
						Print($"Invalid input \"{args[0]}\", must be True or False");
   					}
				}
			}
			if (command is CVar<int> cVarInt)
			{
				if (args.Length == 0)
				{
					Print(cVarInt.Get(context));
				}else {
					try {
						cVarInt.Set(context, Convert.ToInt32(args[0]));

   					}
   					catch (FormatException) {
						Print($"Invalid input \"{args[0]}\", must be an integer");
   					}
				}
			}
			if (command is CVar<float> cVarFloat)
			{
				if (args.Length == 0)
				{
					Print(cVarFloat.Get(context));
				}else {
					try {
						cVarFloat.Set(context, float.Parse(args[0], CultureInfo.InvariantCulture));

   					}
   					catch (FormatException) {
						Print($"Invalid input \"{args[0]}\", must be a float");
   					}
				}
			}
		}
	}
	public void Print(object what)
	{
		ConsoleManager.Contents += what;
		ConsoleManager.Contents += '\n';
		Output.ScrollVertical = Output.GetVScrollBar().MaxValue;

	}


	
}
