using Godot;
using System;

public partial class Module : RigidBody2D
{
    public bool grabbed = false;
    bool mouse = false;
    bool isMouseIn = false;
    bool dragging = false;
    private StaticBody2D cursorBody;
    private PinJoint2D joint;
    private Vector2 lastMouse;
    [Export] CollisionShape2D collider;
    [Export] public float maxAngularSpeed = 5;
    [Export] Node2D edge;
    bool connected = false;
    public override void _PhysicsProcess(double delta)
    {
        GD.Print(mouse);
        if (mouse && Input.IsActionPressed("LCLICK") && !connected)
        {
            Position = GetGlobalMousePosition();
            if (!dragging)
            {
                BeginDrag();
            }
        }
        else
        {
            if (dragging && !connected)
            {
                EndDrag();
            }
        }
        if (!Input.IsActionPressed("LCLICK") && !isMouseIn)
        {
            mouse = false;
        }
        if (cursorBody != null)
        {
            cursorBody.GlobalPosition = GetGlobalMousePosition();
            int dir = 0;
            if (Input.IsActionPressed("RLEFT")) dir--;
            if (Input.IsActionPressed("RRIGHT")) dir++;
            if (dir != 0)
            {
                AngularVelocity = dir * maxAngularSpeed;
            }
        }
    }

    public void OnMouseEntered()
    {
        mouse = true;
        isMouseIn = true;
    }
    public void OnMouseExit()
    {
        isMouseIn = false;
    }

    private void BeginDrag()
    {
        collider.Disabled = true;
        dragging = true;
        cursorBody = new StaticBody2D();
        GetTree().CurrentScene.AddChild(cursorBody);
        cursorBody.GlobalPosition = GetGlobalMousePosition();
        if (IsInstanceValid(joint))
        {
            joint.QueueFree();
        }
        joint = new PinJoint2D
        {
            NodeA = cursorBody.GetPath(),
            NodeB = GetPath(),
            GlobalPosition = cursorBody.GlobalPosition,
            DisableCollision = true,
            Bias = 0.4f,
            Softness = 0.0f
        };
        GetTree().CurrentScene.AddChild(joint);
    }
    private void EndDrag()
    {
        dragging = false;
        collider.Disabled = false;
        Vector2 impulse = (lastMouse - GlobalPosition) * Mass * 0.001f;
        impulse = impulse.Normalized() * Mathf.Clamp(impulse.Length(), 0, 5);
        // ApplyCentralImpulse(impulse);

        joint.QueueFree();
        cursorBody.QueueFree();
        joint = null;
        cursorBody = null;
        dragging = false;
    }

    public void OnTouch(Node2D node)
    {
        if (node.IsInGroup("Connectable") && dragging)
        {

            CallDeferred(nameof(Connect), node);

        }
    }
    public void Connect(Node2D node)
    {
        if (connected)
        {
            return;
        }
        connected = true;
        EndDrag();
        joint = new PinJoint2D
        {
            NodeA = node.GetPath(),
            NodeB = GetPath(),
            GlobalPosition = node.ToLocal(edge.Position),
            DisableCollision = true,
            Bias = 0.75f,
            Softness = 0.0f
        };
        LinearVelocity = Vector2.Zero;
        LockRotation = false;
        node.AddChild(joint);
        // joint.GlobalPosition = Position;
    }
}
