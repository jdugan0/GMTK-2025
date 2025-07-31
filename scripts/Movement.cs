using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Movement : CharacterBody2D
{
    [Export] public float maxSpeed;
    [Export] public float friction;
    [Export] public float accel;
    [Export] PackedScene bullet;
    [Export] float fireSpeed;
    [Export] Node2D firePos;
    [Export] public int bullets = 6;
    [Export] float fireDelay = 1.5f;
    float delayTimer = 0;
    [Export] public AnimatedSprite2D sprite;
    [Export] Node arrowBulletHolder;
    [Export] PackedScene arrowBullet;
    public override void _Ready()
    {
        GameManager.instance.player = this;
        if (bullets > 0)
        {
            sprite.Play("reload");
            AudioManager.instance.PlaySFX("reload");
        }
    }
    public void Death()
    {
        QueueFree();
        GameManager.instance.PlayerDeath();
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 inputDir = Input.GetVector("LEFT", "RIGHT", "UP", "DOWN");
        Vector2 targetVelocity = inputDir.Normalized() * maxSpeed;

        if (inputDir.Length() != 0)
        {
            if (!AudioManager.instance.IsPlaying("footsteps"))
            {
                AudioManager.instance.PlaySFX("footsteps");
            }
        }

        float rate = inputDir == Vector2.Zero ? friction : accel;
        Velocity = Velocity.MoveToward(targetVelocity, rate * (float)delta);
        Vector2 mousePos = GetGlobalMousePosition();
        Vector2 toMouse = mousePos - GlobalPosition;
        float targetAng = toMouse.Angle() + Mathf.Pi / 2;
        Rotation = Mathf.LerpAngle(Rotation, targetAng, 20f * (float)delta);
        MoveAndSlide();
    }
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("FIRE") && delayTimer > fireDelay && bullets > 0)
        {
            Fire();
            delayTimer = 0;
        }
        if (Input.IsActionJustPressed("FIRE") && bullets <= 0)
        {
            AudioManager.instance.PlaySFX("noAmmo");
        }

        if (bullets <= 0)
        {
            sprite.Play("empty");
            delayTimer = 0;
        }
        else
        {
            delayTimer += (float)delta;
        }
        List<Vector2> bulletPos = new List<Vector2>();
        foreach (Node n in GetTree().GetNodesInGroup("Bullet"))
        {
            Bullet b = n as Bullet;
            if (b.playerFired && b.stuck)
            {
                bulletPos.Add(b.Position);
            }
        }
    }


    public void Fire()
    {
        bullets--;
        if (bullets > 0)
        {
            sprite.Play("reload");
            AudioManager.instance.PlaySFX("reload");
        }
        Node node = bullet.Instantiate();
        GetTree().CurrentScene.AddChild(node);
        Bullet b = (Bullet)node;
        b.playerFired = true;
        b.stuck = false;
        b.Velocity = fireSpeed * Vector2.Up.Rotated(Rotation) + Velocity;
        b.Rotate(Rotation);
        b.GlobalPosition = firePos.GlobalPosition;
        AudioManager.instance.PlaySFX("crossbowFire");
    }

}
