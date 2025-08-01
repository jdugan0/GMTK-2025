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
    [Export] Node2D arrowBulletHolder;
    [Export] PackedScene arrowBullet;
    [Export] PackedScene exitArrow;
    [Export] float rollTime;
    [Export] float rollSpeed;
    float rollTImer;
    [Export] float rollCooldown;
    float rollCooldownTimer;
    Node2D arrow = null;
    bool die = false;
    bool spawnedExitArrow = false;
    public bool isRolling;
    uint prevLayer, prevMask;
    Vector2 rollDirection;
    Color origColor;
    float rollRotation;
    public override void _Ready()
    {
        GameManager.instance.player = this;
        if (bullets > 0)
        {
            sprite.Play("reload");
            AudioManager.instance.PlaySFX("reload");
        }
    }
    void StartRoll()
    {
        if (rollCooldownTimer > 0 || isRolling) return;
        origColor = sprite.Modulate;
        // sprite.Modulate = new Color(origColor.R, origColor.G, origColor.B, 0.40f);
        isRolling = true;
        rollTImer = rollTime;
        rollCooldownTimer = rollCooldown;

        prevLayer = CollisionLayer;
        prevMask = CollisionMask;
        CollisionMask &= ~((1u << 1) | (1u << 4));
        CollisionLayer = 1u << 9;

        sprite.Play("roll");

    }
    void EndRoll()
    {
        isRolling = false;
        CollisionLayer = prevLayer;
        CollisionMask = prevMask;
        rollDirection = Vector2.Zero;
        sprite.Modulate = origColor;
        if (bullets > 0)
        {
            sprite.Play("full");
        }
        else
        {
            sprite.Play("empty");
        }
    }

    public void Death()
    {
        if (isRolling) return;
        if (die) return;
        Rotation = 0;
        die = true;
        sprite.Play("death");
        GameManager.instance.PlayerDeath();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (die) return;
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
        if (isRolling)
        {
            if (rollDirection == Vector2.Zero)
            {
                rollDirection = inputDir.Normalized();
                rollRotation = inputDir.Angle() + Mathf.Pi / 2;
            }
            Rotation = rollRotation;
            Velocity = rollDirection * rollSpeed;
        }
        MoveAndSlide();
    }
    public override void _Process(double delta)
    {
        if (die) return;
        if (Input.IsActionJustPressed("FIRE") && delayTimer > fireDelay && bullets > 0)
        {
            Fire();
            delayTimer = 0;
        }
        if (Input.IsActionJustPressed("ROLL"))
        {
            StartRoll();
        }
        if (isRolling)
        {
            rollTImer -= (float)delta;
            if (rollTImer <= 0f)
                EndRoll();
        }

        if (rollCooldownTimer > 0f)
            rollCooldownTimer -= (float)delta;
        if (Input.IsActionJustPressed("FIRE") && bullets <= 0)
        {
            AudioManager.instance.PlaySFX("noAmmo");
        }

        if (bullets <= 0 && !isRolling)
        {
            sprite.Play("empty");
            delayTimer = 0;
        }
        else
        {
            delayTimer += (float)delta;
        }
        if (IsInstanceValid(arrow))
        {
            Vector2 toPlayer = (GetTree().GetNodesInGroup("Exit")[0] as Node2D).GlobalPosition - GlobalPosition;
            arrow.Rotation = toPlayer.Angle() - Rotation;
        }
        if (GameManager.instance.enemiesRemaining > 0)
        {
            UpdateBulletArrows();
        }
        else
        {
            if (spawnedExitArrow)
            {
                return;
            }
            spawnedExitArrow = true;
            while (arrowBulletHolder.GetChildCount() > 0)
            {
                var child = arrowBulletHolder
                    .GetChild(arrowBulletHolder.GetChildCount() - 1);
                arrowBulletHolder.RemoveChild(child);
                child.QueueFree();
            }
            var inst = exitArrow.Instantiate();
            arrowBulletHolder.AddChild(inst);
            arrow = (Node2D)inst;
        }
    }
    public void UpdateBulletArrows()
    {
        List<Vector2> bulletPos = new List<Vector2>();
        foreach (Node n in GetTree().GetNodesInGroup("Bullet"))
        {
            Bullet b = n as Bullet;
            if (b.playerFired && b.stuck)
            {
                bulletPos.Add(b.Position);
            }
        }
        while (arrowBulletHolder.GetChildCount() < bulletPos.Count)
        {
            var arrow = arrowBullet.Instantiate<Node2D>();
            arrowBulletHolder.AddChild(arrow);
        }
        while (arrowBulletHolder.GetChildCount() > bulletPos.Count)
        {
            var child = arrowBulletHolder
                .GetChild(arrowBulletHolder.GetChildCount() - 1);
            arrowBulletHolder.RemoveChild(child);
            child.QueueFree();
        }
        for (int i = 0; i < bulletPos.Count; i++)
        {
            var arrow = (Node2D)arrowBulletHolder.GetChild(i);
            Vector2 toPlayer = bulletPos[i] - GlobalPosition;
            arrow.Rotation = toPlayer.Angle() - Rotation;
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
        if (bullets == 0)
        {
            AudioManager.instance.PlaySFX("crossbowFinal");
        }
        else
        {
            AudioManager.instance.PlaySFX("crossbowFire");
        }
    }

}
