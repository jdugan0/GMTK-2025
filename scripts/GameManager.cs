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
    [Signal] public delegate void EnemyHitEventHandler();
    public OEL oel;
    [Export] public int maxLevelUnlocked = 1;
    public Camera camera;
    public string currentSong;
    [Export] string[] songNames;
    public bool timer;
    private const string SavePath = "user://save.cfg";
    public override void _Ready()
    {
        instance = this;
        LoadProgress();
    }
    public void SaveProgress()
    {
        var cfg = new ConfigFile();
        cfg.Load(SavePath);
        cfg.SetValue("progress", "max_level_unlocked", maxLevelUnlocked);
        cfg.SetValue("options", "volume", AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("Master")));
        cfg.SetValue("options", "timer", timer);
        cfg.SetValue("options", "hardCore", hardCore);
        cfg.Save(SavePath);
    }
    public void LoadProgress()
    {
        var cfg = new ConfigFile();
        var err = cfg.Load(SavePath);
        if (err == Error.Ok)
        {
            maxLevelUnlocked = (int)cfg.GetValue("progress", "max_level_unlocked", maxLevelUnlocked);
            timer = (bool)cfg.GetValue("options", "timer", timer);
            AudioServer.SetBusVolumeLinear(AudioServer.GetBusIndex("Master"), (float)cfg.GetValue("options", "volume", 0));
            hardCore = (bool)cfg.GetValue("options", "hardCore", hardCore);
        }
        else
        {
            SaveProgress();
        }
    }
    public bool hardCore = false;

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
        DialogueManager.instance.ClearQueue();
        await SceneSwitcher.instance.SwitchSceneAsyncSlide("MainMenu");
        currentLevel = "";
        currentLevelID = -1;
        SpeedrunManager.totalTime = 0;
        SpeedrunManager.levelTime = 0;
        SpeedrunManager.speedrun = false;
        CancelMusic();
    }
    public void NextLevel()
    {
        GetTree().Paused = false;
        if (GameManager.instance.maxLevelUnlocked == GameManager.instance.currentLevelID)
        {
            GameManager.instance.maxLevelUnlocked++;
            SaveProgress();
        }
        LoadLevel(currentLevelID + 1);
        DialogueManager.instance.ClearQueue();
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
