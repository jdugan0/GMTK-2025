using Godot;
using System;

public partial class LevelUI : Node
{
    [Export] public string[] levelNames;
    public async void LoadLevel(int id)
    {
        await SceneSwitcher.instance.SwitchSceneAsyncSlide(levelNames[id - 1]);
    }
    public async void Back()
    {
        await SceneSwitcher.instance.SwitchSceneAsyncSlide("MainMenu");
    }
}
