using Godot;
using System;

public partial class PauseMenu : CanvasLayer
{
    
    public override void _Ready()
    {
        GameManager.instance.pauseMenu = this;
    }


    public void MainMenu()
    {
        GameManager.instance.MainMenu();
    }
    public void Resume()
    {
        GameManager.instance.Resume();
    }
}
