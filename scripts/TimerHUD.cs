using Godot;
using System;

public partial class TimerHUD : Label
{
    public override void _Ready()
    {
        if (!GameManager.instance.timer)
        {
            QueueFree();
        }
    }
    public override void _Process(double delta)
    {
        Text = "TIME: " + FormatTime(SpeedrunManager.levelTime);
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
