using Godot;
using System;

public partial class Movement : CharacterBody2D
{
    [Export] public float maxSpeed;
    [Export] public float friction;
    [Export] public float accel;
    public override void _PhysicsProcess(double delta)
    {
        Vector2 inputDir = Input.GetVector("LEFT", "RIGHT", "UP", "DOWN");
        Vector2 targetVelocity = inputDir * maxSpeed;

        float rate = inputDir == Vector2.Zero ? friction : accel;
        Velocity = Velocity.MoveToward(targetVelocity, rate * (float)delta);
        Vector2 mousePos = GetGlobalMousePosition();
        Vector2 toMouse = mousePos - GlobalPosition;
        float targetAng = toMouse.Angle();
        Rotation = Mathf.LerpAngle(Rotation, targetAng, 20f * (float)delta);
        MoveAndSlide();
    }

}
