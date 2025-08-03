using Godot;
using System;

public partial class Camera : Camera2D
{
    [Export] private float pulseAlpha = 0.35f;
    [Export] private float pulseTime = 0.5f;
    [Export] private TextureRect overlay;
    private Tween tween;
    private Tween tween2;
    private bool pulsing = false;
    [Export] public Panel quickPulse;
    public override void _Ready()
    {
        if (IsInstanceValid(GameManager.instance.player))
        {
            GlobalPosition = GameManager.instance.player.GlobalPosition;
        }
        CallDeferred(nameof(SetupSignal));
        GameManager.instance.camera = this;
        Zoom = new Vector2(0.25f, 0.25f);
        PositionSmoothingEnabled = true;
        PositionSmoothingSpeed = 10;
    }
    public void SetupSignal()
    {
        GameManager.instance.player.OneDamage += QuickPulse;
    }

    public async void QuickPulse()
    {
        GD.Print("hi");
        quickPulse.Visible = true;
        var timer = new Timer();
        timer.OneShot = true;
        timer.WaitTime = 0.4;
        AddChild(timer);
        timer.Start();
        await ToSignal(timer, Timer.SignalName.Timeout);
        Color v = Modulate;
        tween2 = GetTree().CreateTween();
        tween2.TweenProperty(quickPulse, "modulate:a", 0.0f, 0.2).From(v.A);
        await ToSignal(tween2, Tween.SignalName.Finished);
        quickPulse.Visible = false;
        quickPulse.Modulate = v;
    }

    public override void _ExitTree()
    {
        tween?.Kill();
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
    public void StartPulse()
    {
        if (pulsing) return;
        pulsing = true;
        tween = GetTree().CreateTween().SetLoops();
        tween.TweenProperty(overlay, "modulate:a", 0.0f, pulseTime).From(pulseAlpha);
        tween.TweenProperty(overlay, "modulate:a", pulseAlpha, pulseTime);
    }

    public void StopPulse()
    {
        if (!pulsing) return;
        pulsing = false;
        tween?.Kill();
        overlay.Modulate = overlay.Modulate with { A = 0 };
    }

}
