#if CLIENT || GAME
using ExtractIntoVoid.Managers;
using Godot;

public partial class SettingsMenu : Control
{
    // menubutton and label lists
    [Export]
    public MenuButton WindowModeMenuButton;

    [Export]
    public Label WindowModeMenuLabel;




    public override void _Ready()
	{
		GameManager.Instance.UIManager.LoadScreenStop();
        WindowModeMenuButton.GetPopup().IdPressed += SettingsMenu_IdPressed;

    }

    public override void _ExitTree()
    {
        WindowModeMenuButton.GetPopup().IdPressed -= SettingsMenu_IdPressed;
    }

    public void Cancel()
	{

	}

    public void Apply()
    {
        this.GetWindow().Borderless = false;
        this.GetWindow().Exclusive = false;
        this.GetWindow().Size = new Vector2I(1920,2010);
        //AudioServer.SetBusVolumeDb(0,0);
    }

    public void Back()
    {

    }

    #region X

    private void SettingsMenu_IdPressed(long id)
    {
        WindowModeMenuLabel.Text = WindowModeMenuButton.GetPopup().GetItemText((int)id);
    }


    #endregion
}
#endif