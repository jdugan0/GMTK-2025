using Godot;
using System;

public partial class LightFlicker : PointLight2D
{
    [Export] public float minInterval = 6.0f;
    [Export] public float maxInterval = 18.0f;
    [Export] public float flickerDuration = 0.20f;
    [Export] public float maxDip = 0.5f;
    private Timer timer;
    private RandomNumberGenerator rng = new RandomNumberGenerator();
    private Tween tween;
    public override void _Ready()
    {
        rng.Randomize();

        // One-shot timer that we restart with a new random wait each time.
        timer = new Timer { OneShot = true };
        AddChild(timer);
        timer.Timeout += OnTimerTimeout;
        ScheduleNext();
    }

    private void ScheduleNext()
    {
        timer.WaitTime = rng.RandfRange(minInterval, maxInterval);
        timer.Start();
    }

    private void OnTimerTimeout()
    {
        FlickerOnce();
        ScheduleNext();
    }
    private void FlickerOnce()
    {
        float baseEnergy = Energy;
        float depth = rng.RandfRange(0.10f, Mathf.Clamp(maxDip, 0.05f, 0.9f));
        float dipTarget = MathF.Max(0.0f, baseEnergy * (1.0f - depth));
        if (tween != null && tween.IsValid())
            tween.Kill();

        tween = CreateTween()
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.InOut);

        tween.TweenProperty(this, "energy", dipTarget, flickerDuration * 0.35f);
        tween.TweenProperty(this, "energy", baseEnergy, flickerDuration * 0.50f);

        float over = baseEnergy * (1.0f + rng.RandfRange(0.02f, 0.06f));
        tween.TweenProperty(this, "energy", over, flickerDuration * 0.20f);
        tween.TweenProperty(this, "energy", baseEnergy, flickerDuration * 0.15f);
    }

}
