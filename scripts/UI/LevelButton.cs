using Godot;
using System;

public partial class LevelButton : TextureButton
{
    [Export] public int level;
    public void LoadLevel()
    {
        GameManager.instance.LoadLevel(level);
    }
    public override void _Ready()
    {
        int levelUnlocked = GameManager.instance.hardCore ? GameManager.instance.maxLevelUnlockedHardcore : GameManager.instance.maxLevelUnlocked;
        if (levelUnlocked < level)
        {
            Disabled = true;
        }
    }

}
