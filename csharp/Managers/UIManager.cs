using Godot;

namespace ExtractIntoVoid.Managers;

public class UIManager
{
    public Control LoadingScreen;

    public void LoadScreenStart()
    {
        LoadingScreen.Show();
    }

    public void LoadScreenStop() 
    {
        LoadingScreen.Hide();
    }
}
