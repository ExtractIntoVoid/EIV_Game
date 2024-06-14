#if CLIENT || GAME
using ExtractIntoVoid.Managers;
using Godot;

public partial class SettingsMenu : Control
{
	public override void _Ready()
	{
		GameManager.Instance.UIManager.LoadScreenStop();
	}


	public void Save()
	{

	}

    public void Apply()
    {
        this.GetWindow().Borderless = false;
        this.GetWindow().Exclusive = false;
        this.GetWindow().Size = new Vector2I(1920,2010);  
    }

    public void Back()
    {

    }

    #region X
    #endregion
}
#endif