using Godot;
using System;
public partial class Bullet : CharacterBody2D
{

    private bool stuck = false;
    [Export] private CollisionShape2D pickupRadius;
    [Export] private CollisionShape2D shape;

    public override void _PhysicsProcess(double delta)
    {
        var hit = MoveAndCollide(Velocity * (float)delta);
        if (hit != null)
            ResolveHit(hit);
        if (stuck)
        {
            pickupRadius.Disabled = false;
            shape.Disabled = true;
        }
    }

    private void ResolveHit(KinematicCollision2D hit)
    {
        var target = hit.GetCollider() as Node2D;
        if (target.IsInGroup("hittable") == true && !stuck)
        {
            (target as Enemy)?.Death();
            Velocity = Vector2.Zero;
            stuck = true;
            return;
        }
    }
    public void ResolvePickup(Node2D hit)
    {
        GD.Print("hi");
        if (hit is Movement player && stuck)
        {
            player.bullets++;
            QueueFree();
        }
    }
}
