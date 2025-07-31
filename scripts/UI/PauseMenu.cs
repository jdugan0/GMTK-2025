using Godot;
using System;

public partial class PauseMenu : CanvasLayer
{
    
    public override void _Ready()
    {
        GameManager.instance.pauseMenu = this;
    }

    public async void MainMenu()
    {
        GetTree().Paused = false;
        await SceneSwitcher.instance.SwitchSceneAsyncSlide("MainMenu");
        GameManager.instance.CancelMusic();
    }
    public void Resume()
    {
        GameManager.instance.Resume();
    }
}
