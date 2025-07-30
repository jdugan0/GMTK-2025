using Godot;
using System;

public partial class Camera : Camera2D
{
    public override void _Process(double delta)
    {
        if (IsInstanceValid(GameManager.instance.player))
        {
            Position = GameManager.instance.player.Position;
        }
    }

}
