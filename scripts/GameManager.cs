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
    public int currentLevelID = -1;
    public int enemiesRemaining;
    public int health;
    public string[] levels;
    public OEL oel;
    [Export] public int maxLevelUnlocked = 1;
    public Camera camera;
    public string currentSong;
    [Export] string[] songNames;
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
        CancelMusic();
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
        AudioManager.instance.CancelSFX("mainMenu");
        await SceneSwitcher.instance.SwitchSceneAsyncSlide(levels[id - 1]);
        await GameManager.instance.StartMusic(songNames[id - 1]);
        SpeedrunManager.levelTime = 0;
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

    public async Task StartMusic(string songName)
    {
        if (AudioManager.instance.IsPlaying(songName) || AudioManager.instance.IsPlaying(songName + "Intro")) return;
        AudioManager.instance.CancelSFX(currentSong);
        AudioManager.instance.CancelSFX(currentSong + "Intro");
        currentSong = songName;

        if (AudioManager.instance.SoundExists(songName + "Intro"))
        {
            AudioStreamPlayer introPlayer = AudioManager.instance.PlaySFX(songName + "Intro");
            await ToSignal(introPlayer, AudioStreamPlayer.SignalName.Finished);
        }
        AudioManager.instance.PlaySFX(songName);
    }

    public void CancelMusic()
    {
        AudioManager.instance.CancelSFX(currentSong);
        AudioManager.instance.CancelSFX(currentSong + "Intro");
    }


}
