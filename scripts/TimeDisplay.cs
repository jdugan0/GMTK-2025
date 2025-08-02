using Godot;
using System;

public partial class TimeDisplay : Label
{
    [Export] public bool levelTime;
    public override void _Ready()
    {
        if (!levelTime)
        {
            Text = "TOTAL TIME: " + FormatTime(SpeedrunManager.totalTime);
        }
    }

    public override void _Process(double delta)
    {
        if (levelTime)
        {
            Text = "LEVEL TIME: " + FormatTime(SpeedrunManager.levelTime);
        }
    }



    private string FormatTime(double totalSeconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(totalSeconds);
        return string.Format("{0:D2}:{1:D2}:{2:D3}",
            (int)time.TotalMinutes,
            time.Seconds,
            time.Milliseconds);
    }


}
