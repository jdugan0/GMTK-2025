using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;

public partial class GameManager : Node
{
    public Movement player;
    public static GameManager instance;
    public PauseMenu pauseMenu;
    public bool paused = false;
    public string currentLevel;
    public int currentLevelID;
    public int enemiesRemaining;
    public string[] levels;
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
        if (Input.IsActionJustPressed("RESTART"))
        {
            RestartLevel();
        }
    }
    public async void MainMenu()
    {
        GetTree().Paused = false;
        await SceneSwitcher.instance.SwitchSceneAsyncSlide("MainMenu");
        instance.CancelMusic();
    }
    public async void NextLevel()
    {
        GetTree().Paused = false;
        if (currentLevelID == levels.Length)
        {
            await SceneSwitcher.instance.SwitchSceneAsyncSlide("GameComplete");
        }
        else
        {
            LoadLevel(currentLevelID + 1);
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
    public async void LoadLevel(int id)
    {
        GameManager.instance.currentLevel = levels[id - 1];
        GameManager.instance.currentLevelID = id;
        await SceneSwitcher.instance.SwitchSceneAsyncSlide(levels[id - 1]);
        await GameManager.instance.StartMusic();
    }
    public async void RestartLevel()
    {
        await SceneSwitcher.instance.SwitchSceneAsyncSlide(currentLevel);
    }
    public async void PlayerDeath()
    {
        AudioStreamPlayer player = AudioManager.instance.PlaySFX("playerDeath");
        await ToSignal(player, AudioStreamPlayer.SignalName.Finished);
        RestartLevel();
    }

    public async Task StartMusic()
    {
        AudioStreamPlayer introPlayer = AudioManager.instance.PlaySFX("lvl1Intro");
        await ToSignal(introPlayer, AudioStreamPlayer.SignalName.Finished);
        AudioManager.instance.PlaySFX("lvl1Main");
    }

    public void CancelMusic()
    {
        AudioManager.instance.CancelSFX("lvl1Intro");
        AudioManager.instance.CancelSFX("lvl1Main");
    }


}
