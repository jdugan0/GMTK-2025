using Godot;
using System;

public partial class SpeedrunManager : Node
{
    public static float levelTime;
    public static float totalTime;
    public static bool speedrun;
    public override void _Process(double delta)
    {
        if (GameManager.instance.currentLevelID == 1)
        {
            speedrun = true;
        }
        if (GameManager.instance.currentLevelID != -1)
        {
            if (speedrun)
            {
                totalTime += (float)delta;
            }
            levelTime += (float)delta;
        }
    }

}
