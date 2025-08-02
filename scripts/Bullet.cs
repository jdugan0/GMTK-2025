using Godot;
using System;
public partial class Bullet : CharacterBody2D
{

    [Export] public bool stuck = false;
    [Export] private CollisionShape2D pickupRadius;
    [Export] private CollisionShape2D shape;
    [Export] AnimatedSprite2D sprite;
    [Export] PackedScene splinterBurst;
    [Export] GpuParticles2D trail;
    public bool playerFired = false;
    float timeStuck = 0f;
    public void SplinterBurst(KinematicCollision2D hit)
    {
        GpuParticles2D particles = splinterBurst.Instantiate<GpuParticles2D>();
        if (splinterBurst == null) return;

        var p = splinterBurst.Instantiate<GpuParticles2D>();
        GetTree().CurrentScene.AddChild(p);
        p.GlobalPosition = hit.GetPosition();
        var n = hit.GetNormal().Normalized();
        var incoming = Velocity.Length() > 0 ? Velocity.Normalized() : -n;
        var reflect = incoming.Bounce(n).Normalized();
        var dir = (n * 0.7f + reflect * 0.3f).Normalized();
        if (p.ProcessMaterial is ParticleProcessMaterial mat)
            mat.Direction = new Vector3(dir.X, dir.Y, 0);
        p.Emitting = true;
        p.Finished += () => p.QueueFree();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (stuck)
        {
            trail.Emitting = false;
        }
        var hit = MoveAndCollide(Velocity * (float)delta);
        if (hit != null)
            ResolveHit(hit);
        if (stuck)
        {
            pickupRadius.Disabled = false;
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
                SplinterBurst(hit);
            }
            Velocity = Vector2.Zero;
            stuck = true;
            return;
        }
    }
    public void Hit()
    {
        Velocity = Vector2.Zero;
        stuck = true;
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
