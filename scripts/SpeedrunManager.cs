using Godot;
using System;

public partial class SpeedrunManager : Node
{
    public static float levelTime;
    public static float totalTime;
    public override void _Process(double delta)
    {
        totalTime += (float)delta;
        levelTime += (float)delta;
    }

}
