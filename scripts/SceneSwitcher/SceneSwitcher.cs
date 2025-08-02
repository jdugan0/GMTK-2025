using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class SceneSwitcher : Node
{
    public static SceneSwitcher instance = null;
    [Export] public SceneResource[] scenes;
    [Export] private Control fadeRect;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        instance = this;
    }
    public async Task SwitchSceneAsyncSlide(string sceneName)
    {
        AudioManager.instance.PlaySFX("wooshIn");
        await SlideIn(0.35);
        GetTree().ChangeSceneToPacked(scenes[Array.FindIndex(scenes, s => s.sceneName == sceneName)].scene);
        await WaitOneFrame();
        AudioManager.instance.PlaySFX("wooshOut");
        await SlideOut(0.35);
        fadeRect.Visible = false;
    }

    public void SwitchScene(int loadOrder)
    {
        GetTree().ChangeSceneToPacked(scenes[loadOrder].scene);
    }
    public async Task WaitOneFrame()
    {
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
    }

    public void SwitchScene(string sceneName)
    {
        GetTree().ChangeSceneToPacked(
            scenes[Array.FindIndex(scenes, s => s.sceneName == sceneName)].scene
        );
    }

    private async Task SlideIn(double dur)
    {
        var size = GetViewport().GetVisibleRect().Size;
        fadeRect.Visible = true;
        fadeRect.Position = new Vector2(-size.X, 0); // start off-screen left
        var t = GetTree().CreateTween();
        t.TweenProperty(fadeRect, "position:x", 0, dur)
         .SetTrans(Tween.TransitionType.Cubic)
         .SetEase(Tween.EaseType.InOut);
        await ToSignal(t, Tween.SignalName.Finished);
    }

    private async Task SlideOut(double dur)
    {
        var size = GetViewport().GetVisibleRect().Size;
        var t = GetTree().CreateTween();
        t.TweenProperty(fadeRect, "position:x", size.X, dur)
         .SetTrans(Tween.TransitionType.Cubic)
         .SetEase(Tween.EaseType.InOut);
        await ToSignal(t, Tween.SignalName.Finished);
    }


}
