using Godot;
using System;

public partial class EnemiesRemaining : Label
{
    int originalCount = 0;
    public override void _Ready()
    {
        originalCount = GetTree().GetNodesInGroup("Enemy").Count;
    }

    public override void _Process(double delta)
    {
        Text = GetTree().GetNodesInGroup("Enemy").Count + " / " + originalCount;
    }

}
