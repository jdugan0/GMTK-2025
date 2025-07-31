using Godot;
using System;

public partial class MainMenu : Control
{
    public override void _Ready()
    {
        AudioManager.instance.PlaySFX("mainMenu");
    }

    public void Quit()
    {
        GetTree().Quit();
    }
    public async void Levels()
    {
        await SceneSwitcher.instance.SwitchSceneAsyncSlide("levels");
    }
}
