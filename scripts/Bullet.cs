using Godot;
using System;
public partial class Bullet : CharacterBody2D
{
    public enum BulletType
    {
        Normal,
        Pierce
    }

    [Export] public bool stuck = false;
    [Export] private CollisionShape2D pickupRadius;
    [Export] private CollisionShape2D shape;
    [Export] AnimatedSprite2D sprite;
    [Export] PackedScene splinterBurst;
    [Export] GpuParticles2D trail;
    [Export] public BulletType bulletType = BulletType.Normal;
    public bool playerFired = false;
    public bool hitEnemy = false;
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
            CollisionLayer = 1 << (15 - 1);
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
                if (bulletType != BulletType.Pierce || hitEnemy)
                {
                    Velocity = Vector2.Zero;
                    stuck = true;
                }
                hitEnemy = true;
            }
            else
            {
                AudioManager.instance.PlaySFX("hitFail");
                SplinterBurst(hit);
                Velocity = Vector2.Zero;
                stuck = true;
            }

            return;
        }
    }
    public void Hit()
    {
        if (bulletType != BulletType.Pierce || hitEnemy)
        {
            Velocity = Vector2.Zero;
            stuck = true;
        }
        hitEnemy = true;
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
            player.AddBullet(bulletType);
            QueueFree();
        }
    }
}
