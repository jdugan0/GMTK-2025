using Godot;
using System;

public partial class HardCoreFreeUI : Control
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
