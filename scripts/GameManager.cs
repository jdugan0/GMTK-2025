using Godot;
using System;
using System.Threading.Tasks;

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
        if (!IsInstanceValid(pauseMenu)) return;
        if (Input.IsActionJustPressed("PAUSE"))
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

    public async Task StartMusic()
    {
        AudioStreamPlayer introPlayer = AudioManager.instance.PlaySFX("lvl1Intro");
        await ToSignal(introPlayer, AudioStreamPlayer.SignalName.Finished);
        AudioManager.instance.PlaySFX("lvl1Main");
    }


}
