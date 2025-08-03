using Godot;
using System;

public partial class Health : Node
{
    public int health = 3;
    public override void _Process(double delta)
    {
        if (!GameManager.instance.hardCore && GameManager.instance.player.health != health)
        {
            var child = GetChild(0);
            RemoveChild(child);
            child.QueueFree();
            health--;
        }
    }
    public override void _Ready()
    {
        if (GameManager.instance.hardCore)
        {
            while (GetChildCount() > 0)
            {
                var child = GetChild(0);
                RemoveChild(child);
                child.QueueFree();
            }
        }
    }


}
