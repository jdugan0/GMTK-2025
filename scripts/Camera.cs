using Godot;
using System;

public partial class Camera : Camera2D
{
    [Export] private float pulseAlpha = 0.35f;
    [Export] private float pulseTime = 0.5f;
    private ColorRect overlay;
    private Tween tween;
    private bool pulsing = false;
    public override void _Ready()
    {
        if (IsInstanceValid(GameManager.instance.player))
        {
            GlobalPosition = GameManager.instance.player.GlobalPosition;
        }
        Zoom = new Vector2(0.25f, 0.25f);
        PositionSmoothingEnabled = true;
        PositionSmoothingSpeed = 10;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (IsInstanceValid(GameManager.instance.player))
        {
            GlobalPosition = GameManager.instance.player.GlobalPosition;

            if (GameManager.instance.player.health == 1 && !pulsing)
                StartPulse();
            else if (GameManager.instance.player.health > 1 && pulsing)
                StopPulse();
        }
    }
    private void StartPulse()
    {
        pulsing = true;
        tween = GetTree().CreateTween().SetLoops();
        tween.TweenProperty(overlay, "modulate:a", 0.0f, pulseTime).From(pulseAlpha);
        tween.TweenProperty(overlay, "modulate:a", pulseAlpha, pulseTime);
    }

    private void StopPulse()
    {
        pulsing = false;
        tween?.Kill();
        overlay.Modulate = overlay.Modulate with { A = 0 }; 
    }

}
