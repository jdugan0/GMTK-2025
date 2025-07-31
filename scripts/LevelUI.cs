using Godot;
using System;

public partial class LevelUI : Node
{
    [Export] public string[] levelNames;
    public void LoadLevel(int id)
    {
        SceneSwitcher.instance.SwitchScene(levelNames[id - 1]);
    }
}
