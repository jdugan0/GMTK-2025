using Godot;
using System;

public partial class BaseModule : RigidBody2D
{
    private Module[] modules;
    [Export] public float MoveTorque = 250f;
    [Export] public float MaxAngular = 8f;
    public override void _Ready()
    {
        GameManager.instance.player = this;
    }

    public override void _PhysicsProcess(double delta)
    {
        float input = Input.GetAxis("LEFT", "RIGHT");
        if (Mathf.IsZeroApprox(input)) return;

        if (Mathf.Abs(AngularVelocity) < MaxAngular)
            ApplyTorque(input * MoveTorque);
    }

}
