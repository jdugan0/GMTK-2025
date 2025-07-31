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
        if (!IsInstanceValid(GameManager.instance.player)) return;
        nav.TargetPosition = GameManager.instance.player.GlobalPosition;
        if (!nav.IsTargetReached())
        {
            var dir = ToLocal(nav.GetNextPathPosition()).Normalized();
            Velocity = dir * maxSpeed;
            MoveAndSlide();
        }
        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            var c = GetSlideCollision(i);
            OnCol(c.GetCollider() as Node2D);
        }
    }

    public void OnCol(Node2D node)
    {
        if (node is Movement)
        {
            node.QueueFree();
        }
    }

}
