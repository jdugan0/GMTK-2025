using Godot;
using System;

public partial class Camera : Camera2D
{
    public override void _Ready()
    {
        if (IsInstanceValid(GameManager.instance.player))
        {
            GlobalPosition = GameManager.instance.player.GlobalPosition;
        }
        PositionSmoothingEnabled = true;
        PositionSmoothingSpeed = 5;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (IsInstanceValid(GameManager.instance.player))
        {
            GlobalPosition = GameManager.instance.player.GlobalPosition;
        }
    }

}
