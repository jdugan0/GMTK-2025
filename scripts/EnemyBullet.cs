using Godot;
using System;

public partial class EnemyBullet : CharacterBody2D
{
    public override void _PhysicsProcess(double delta)
    {
        var hit = MoveAndCollide(Velocity * (float)delta);
        if (hit != null)
            ResolveHit(hit);
    }
    private void ResolveHit(KinematicCollision2D hit)
    {
        var target = hit.GetCollider() as Node2D;
        if (target.IsInGroup("hittable") == true)
        {

            if (target is Movement enemy)
            {
                enemy.Death();
                QueueFree();
            }
            else
            {
                QueueFree();
            }
            return;
        }
    }
}
