using Godot;
using System;

public partial class LevelEnd : Area2D
{
    [Export] CanvasLayer UI;
    public void OnCol(Node2D node)
    {
        if (node is Movement && GameManager.instance.enemiesRemaining == 0)
        {
            GetTree().Paused = true;
            UI.Visible = true;
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
