using Godot;
using System;

public partial class MainMenu : Control
{
    public void Quit()
    {
        GetTree().Quit();
    }
    public async void Levels()
    {
        await SceneSwitcher.instance.SwitchSceneAsyncSlide("levels");
    }
}
