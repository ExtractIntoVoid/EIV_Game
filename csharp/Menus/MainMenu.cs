using ExtractIntoVoid.Managers;
using Godot;

public partial class MainMenu : Control
{
	public override void _Ready()
	{
		GameManager.Instance.UIManager.LoadScreenStop();
	}


}
