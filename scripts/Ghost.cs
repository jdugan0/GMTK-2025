using Godot;
using System;

public partial class Ghost : AnimatedSprite2D
{
    [Export] float speed;
    public override void _Ready()
    {
        Play("animation");
    }

    public override void _Process(double delta)
    {
        Position += Vector2.Up * speed * (float)delta;
    }

}
