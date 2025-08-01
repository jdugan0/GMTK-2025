using Godot;
using System;

public partial class LevelEnd : Area2D
{
    [Export] CanvasLayer UI;
    [Export] AnimatedSprite2D sprite;
    bool complete = false;
    public void OnCol(Node2D node)
    {
        if (node is Movement && GameManager.instance.enemiesRemaining == 0 && !complete)
        {
            GetTree().Paused = true;
            UI.Visible = true;
            if (GameManager.instance.maxLevelUnlocked == GameManager.instance.currentLevelID)
            {
                GameManager.instance.maxLevelUnlocked++;
            }
            complete = true;

        }

    }
    public override void _Process(double delta)
    {
        if (GameManager.instance.enemiesRemaining == 0)
        {
            sprite.Play("open");
        }
        else
        {
            sprite.Play("closed");
        }
    }

    public void MainMenu()
    {
        GameManager.instance.MainMenu();
    }
    public void NextLevel()
    {
        GameManager.instance.NextLevel();
    }
}
