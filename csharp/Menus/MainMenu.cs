#if CLIENT || GAME
using ExtractIntoVoid.Managers;
using Godot;

public partial class MainMenu : Control
{
	public override void _Ready()
	{
		GameManager.Instance.UIManager.LoadScreenStop();
	}

	public void Play()
	{
        this.Hide();
        var MainMenu = GameManager.Instance.SceneManager.GetPackedScene("ConnectionScreen").Instantiate();
        this.CallDeferred("add_sibling", MainMenu);
        GameManager.Instance.UIManager.LoadScreenStart();
    }

	public void Quit()
	{
		GameManager.Instance.Quit();
    }

	public void Settings()
	{


	}

}
#endif