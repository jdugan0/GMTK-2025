using Godot;
using System;

public partial class HarcoreFree : Node2D
{
    public override void _Ready()
    {
        if (!GameManager.instance.hardCore)
        {
            QueueFree();
        }
        else
        {
            Visible = true;
        }
    }

}
