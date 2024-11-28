#if CLIENT || GAME
using ExtractIntoVoid.Client;
using ExtractIntoVoid.Managers;
using ExtractIntoVoid.Menus;
using Godot;

public partial class SettingsMenu : Control
{
    // menubutton and label lists
    [Export]
    public MenuButton MSAA2DMenuButton;

    [Export]
    public MenuButton MSAA3DMenuButton;

    [Export]
    public MenuButton WindowModeMenuButton;

    [Export]
    public MarginContainer AllSettingsContainer;

    [Export]
    public VBoxContainer GameSettingsContainer;

    [Export]
    public VBoxContainer VideoSettings;

    [Export]
    public VBoxContainer ControlsSettings;

    [Export]
    public VBoxContainer AudioSettings;



    public GameSettings Settings;

    public override void _Ready()
	{
        if (ResourceLoader.Exists("user://GameSettings"))
            Settings = ResourceLoader.Load<GameSettings>("user://GameSettings");
        else
            Settings = new GameSettings();

        // set stuff like button things
        GameButton_Pressed();
        WindowModeMenuButton.GetPopup().IdPressed += SettingsMenu_IdPressed;
        AllSettingsContainer.Show();
        //GameManager.Instance.UIManager.LoadScreenStop();
    }

    public override void _ExitTree()
    {
        WindowModeMenuButton.GetPopup().IdPressed -= SettingsMenu_IdPressed;
    }

    public void Cancel()
	{
        // set back the stuff as before.
	}

    public void Apply()
    {
        Settings.Visual.Borderless = VideoSettings.GetNode<CheckBox>("/Borderless/CheckBox").ButtonPressed;
        Settings.Visual.Exclusive = VideoSettings.GetNode<CheckBox>("/Exclusive/CheckBox").ButtonPressed;

        this.GetWindow().Borderless = Settings.Visual.Borderless;
        this.GetWindow().Exclusive = Settings.Visual.Exclusive;
        this.GetWindow().Mode = Settings.Visual.Mode;
        this.GetWindow().Msaa2D = Settings.Visual.MSAA2D;
        this.GetWindow().Msaa3D = Settings.Visual.MSAA3D;
        this.GetWindow().Scaling3DMode = Settings.Visual.Scaling3DMode;
        this.GetWindow().ScreenSpaceAA = Settings.Visual.ScreenSpaceAA;
        switch (Settings.Visual.Scaling3DScale)
        {
            case Scaling3DScaleEnum.Performance:
                this.GetWindow().Scaling3DScale = 0.5f;
                break;
            case Scaling3DScaleEnum.Balanced:
                this.GetWindow().Scaling3DScale = 0.59f;
                break;
            case Scaling3DScaleEnum.Quality:
                this.GetWindow().Scaling3DScale = 0.67f;
                break;
            case Scaling3DScaleEnum.UltraQuality:
                this.GetWindow().Scaling3DScale = 0.77f;
                break;
            default:
                break;
        }
        GD.Print(this.GetWindow().MaxSize);
        switch (Settings.Visual.ScreenSize)
        {
            case ScreenSizeEnum._3840x2160:
                this.GetWindow().Size = new Vector2I(3840, 2160);
                break;
            case ScreenSizeEnum._2560x1440:
                this.GetWindow().Size = new Vector2I(2560, 1440);
                break;
            case ScreenSizeEnum._1920x1080:
                this.GetWindow().Size = new Vector2I(1920, 1080);
                break;
            case ScreenSizeEnum._1280x720:
                this.GetWindow().Size = new Vector2I(1280, 720);
                break;
            default:
                break;
        }
        ResourceSaver.Save(Settings , "user://GameSettings");
        //AudioServer.SetBusVolumeDb(0,0);
    }

    public void Back()
    {
        this.Hide();
        GetParent<MainMenu>().Show();
        this.QueueFree();
    }

    #region Presses

    private void SettingsMenu_IdPressed(long id)
    {
        WindowModeMenuButton.Text = WindowModeMenuButton.GetPopup().GetItemText((int)id);
        WindowModeMenuButton.GetPopup().SetItemChecked((int)id, true);
    }

    public void GameButton_Pressed()
    {
        GD.Print("GameButton_Pressed");
        GameSettingsContainer.Show();
        VideoSettings.Hide();
        ControlsSettings.Hide();
        AudioSettings.Hide();
    }

    public void VideoButton_Pressed()
    {
        GD.Print("VideoButton_Pressed");
        GameSettingsContainer.Hide();
        VideoSettings.Show();
        ControlsSettings.Hide();
        AudioSettings.Hide();
    }

    public void ControlsButton_Pressed()
    {
        GD.Print("ControlsButton_Pressed");
        GameSettingsContainer.Hide();
        VideoSettings.Hide();
        ControlsSettings.Show();
        AudioSettings.Hide();
    }

    public void AudioButton_Pressed()
    {
        GD.Print("AudioButton_Pressed");
        GameSettingsContainer.Hide();
        VideoSettings.Hide();
        ControlsSettings.Hide();
        AudioSettings.Show();
    }

    #endregion
}
#endif