using Godot;
using System;

public partial class Movement : CharacterBody2D
{
    [Export] public float maxSpeed;
    [Export] public float friction;
    [Export] public float accel;
    [Export] PackedScene bullet;
    [Export] float fireSpeed;
    [Export] Node2D firePos;
    public int bullets = 6;
    [Export] float fireDelay = 1.5f;
    float delayTimer = 10000;
    public override void _Ready()
    {
        GameManager.instance.player = this;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 inputDir = Input.GetVector("LEFT", "RIGHT", "UP", "DOWN");
        Vector2 targetVelocity = inputDir.Normalized() * maxSpeed;

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
        delayTimer += (float)delta;
    }


    public void Fire()
    {
        bullets--;
        Node node = bullet.Instantiate();
        GetTree().CurrentScene.AddChild(node);
        Bullet b = (Bullet)node;
        b.Velocity = fireSpeed * Vector2.Up.Rotated(Rotation) + Velocity;
        b.Rotate(Rotation);
        b.GlobalPosition = firePos.GlobalPosition;
    }

}
