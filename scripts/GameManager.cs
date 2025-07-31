using Godot;
using System;

public partial class GameManager : Node
{
    public Movement player;
    public static GameManager instance;
    public PauseMenu pauseMenu;
    public bool paused = false;
    public string currentLevel;
    public override void _Ready()
    {
        instance = this;
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("PAUSE") && IsInstanceValid(pauseMenu))
        {
            if (paused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        pauseMenu.Visible = true;
        GetTree().Paused = true;
        paused = true;
    }
    public void Resume()
    {
        pauseMenu.Visible = false;
        GetTree().Paused = false;
        paused = false;
    }
    public async void RestartLevel()
    {
        await SceneSwitcher.instance.SwitchSceneAsyncSlide(currentLevel);
    }


}
