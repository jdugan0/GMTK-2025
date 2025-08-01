using Godot;
using System;

public partial class OEL : TextureRect
{
    [Export] float initialY = 1080;
    [Export] float finalY;
    [Export] Curve follow;
    private Tween tween;
    [Export] public float duration = 2f;
    public override void _Ready()
    {
        GameManager.instance.oel = this;
    }

    public void PlayMotionOnce()
    {
        tween?.Kill();
        Position = new Vector2(Position.X, initialY);

        tween = GetTree().CreateTween();
        tween.TweenMethod(Callable.From<float>(ApplyYFromT), 0f, 1f, duration);
        tween.TweenCallback(Callable.From(() =>
        {
            Position = new Vector2(Position.X, finalY);
            Position = new Vector2(Position.X, (initialY));
        }));
    }

    private void ApplyYFromT(float t)
    {
        float u = follow != null ? follow.Sample(t) : t; // remap by curve
        float y = Mathf.Lerp(initialY, finalY, u);
        Position = new Vector2(Position.X, y);
    }



}
