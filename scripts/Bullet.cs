using Godot;
using System;
public partial class Bullet : CharacterBody2D
{

    [Export] public bool stuck = false;
    [Export] private CollisionShape2D pickupRadius;
    [Export] private CollisionShape2D shape;
    [Export] AnimatedSprite2D sprite;
    public bool playerFired = false;
    float timeStuck = 0f;

    public override void _PhysicsProcess(double delta)
    {
        var hit = MoveAndCollide(Velocity * (float)delta);
        if (hit != null)
            ResolveHit(hit);
        if (stuck)
        {
            pickupRadius.Disabled = false;
            shape.Disabled = true;
            float scaleFactor = 0.25f * Mathf.Sin(2 * timeStuck) + 1.25f;
            sprite.Scale = new Vector2(scaleFactor, scaleFactor);
            sprite.Animation = "OnGround";
            timeStuck += (float)delta;
        }
    }

    private void ResolveHit(KinematicCollision2D hit)
    {
        var target = hit.GetCollider() as Node2D;
        if (target.IsInGroup("hittable") == true && !stuck)
        {
            if (target is Enemy enemy)
            {
                enemy.Death();
            }
            else
            {
                AudioManager.instance.PlaySFX("hitFail");
            }
            Velocity = Vector2.Zero;
            stuck = true;
            return;
        }
    }
    public void ResolvePickup(Node2D hit)
    {
        
        if (hit is Movement player && stuck)
        {
            if (player.isRolling) return;
            if (player.bullets == 0)
            {
                player.sprite.Play("reload");
                AudioManager.instance.PlaySFX("reload");
            }
            AudioManager.instance.PlaySFX("bulletPickup");
            player.bullets++;
            QueueFree();
        }
    }
}
