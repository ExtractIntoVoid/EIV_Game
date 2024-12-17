#if CLIENT || GAME
using ExtractIntoVoid.Managers;
using Godot;

namespace ExtractIntoVoid.Menus;

public partial class MainMenu : ConsoleEnabledScene
{
	public override void _Ready()
	{
		ConsoleManager.Register("help", new CFunc("Shows all commands.", (ctx) =>
		{
			foreach(var command in ConsoleManager.GetAll())
				ctx.Console.Print($"{command.Key} ({(command.Value is CFunc ? "func" : "var")})\t {command.Value.Description}");
		}));
		ConsoleManager.Register("close", new CFunc("Closes the console window.", (ctx) =>
		{
			ctx.Console.Close();
		}));
		ConsoleManager.Register("version", new CFunc("Prints the game's version", (ctx) =>
		{
			ctx.Console.Print(BuildDefined.FullVersion);
		}));
		ConsoleManager.Register("exit", new CFunc("Closes the game", (ctx) =>
		{
			GetTree().Quit();
		}));
		ConsoleManager.Register("title", new CVar<string>("The console window's title",
		(ctx) =>{
			return ctx.Console.Title;
		},
		(ctx, value) =>{
			ctx.Console.Title = value;
		}));
		
		//AddChild(ConsoleManager.GetConsole());
		//AddChild(SceneManager.GetPackedScene("DevConsole").Instantiate());
		//ConsoleManager.Console.Visible = true;
		GameManager.Instance.UIManager.LoadScreenStop();
		GetNode<Label>("VersionLabel").Text = BuildDefined.FullVersion;
	}

	public void Play()
	{
        this.Hide();
        var MainMenu = SceneManager.GetPackedScene("ConnectionScreen").Instantiate();
        this.CallDeferred("add_sibling", MainMenu);
        GameManager.Instance.UIManager.LoadScreenStart();
    }

	public void Quit()
	{
		GameManager.Instance.Quit();
    }

	public void Settings()
	{
        this.Hide();
        var MainMenu = SceneManager.GetPackedScene("Settings").Instantiate();
        this.CallDeferred("add_sibling", MainMenu);
        GameManager.Instance.UIManager.LoadScreenStart();
    }

}
#endif