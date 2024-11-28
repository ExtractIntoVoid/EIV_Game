#if CLIENT || GAME
using ExtractIntoVoid.Managers;
using Godot;

namespace ExtractIntoVoid.Menus;

public partial class MainMenu : Control
{
	public override void _Ready()
	{
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