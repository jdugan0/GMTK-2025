using Godot;
using System;

public partial class LevelUI : Node
{
    [Export] public string[] levelNames;
    public override void _Ready()
    {
        GameManager.instance.levels = levelNames;
    }

    public void LoadLevel(int id)
    {
        GameManager.instance.LoadLevel(id);
    }
    public async void Back()
    {
        await SceneSwitcher.instance.SwitchSceneAsyncSlide("MainMenu");
    }
}
