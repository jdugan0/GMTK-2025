using Godot;
using System;

public partial class MainMenu : Control
{
    public void Quit()
    {
        GetTree().Quit();
    }
    public void Levels()
    {
        SceneSwitcher.instance.SwitchScene("levels");
    }
}
