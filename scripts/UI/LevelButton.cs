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
        if (GameManager.instance.maxLevelUnlocked < level)
        {
            Disabled = true;
        }
    }

}
