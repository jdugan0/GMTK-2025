using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
    [Export] NavigationAgent2D nav;
    [Export] float maxSpeed;
    public void Death()
    {
        QueueFree();
    }
    public override void _PhysicsProcess(double delta)
    {
        nav.TargetPosition = GameManager.instance.player.GlobalPosition;
        if (!nav.IsTargetReached())
        {
            var dir = ToLocal(nav.GetNextPathPosition()).Normalized();
            Velocity = dir * maxSpeed;
            MoveAndSlide();
        }
    }

}
