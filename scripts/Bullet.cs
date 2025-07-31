using Godot;
using System;
public partial class Bullet : CharacterBody2D
{

    private bool stuck;
    [Export] private CollisionShape2D pickupRadius;
    [Export] private CollisionShape2D shape;

    public override void _PhysicsProcess(double delta)
    {
        var hit = MoveAndCollide(Velocity * (float)delta);
        if (hit != null)
            ResolveHit(hit);
        if (Velocity.Length() == 0)
        {
            pickupRadius.Disabled = false;
            shape.Disabled = true;
        }
    }

    private void ResolveHit(KinematicCollision2D hit)
    {
        var target = hit.GetCollider() as Node2D;
        if (target?.IsInGroup("hittable") == true && !stuck)
        {
            (target as Enemy)?.Death();
            GlobalPosition = hit.GetPosition();
            Velocity = Vector2.Zero;
            stuck = true;
            return;
        }
    }
    public void ResolvePickup(Node2D hit)
    {
        if (hit is Movement player)
        {
            player.bullets++;
            QueueFree();
        }
    }
}
