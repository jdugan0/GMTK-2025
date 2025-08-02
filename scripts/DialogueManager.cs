using Godot;
using System;
using System.Collections.Generic;

public partial class DialogueManager : Node
{
    public static DialogueManager instance;
    [Export] Godot.Collections.Dictionary<string, Dialouge> lines = new Godot.Collections.Dictionary<string, Dialouge>();
    [Export] Texture2D shockedTextBox;
    [Export] Texture2D relievedTextBox;
    [Export] Texture2D afraidTextBox;

    [Export] Control display;
    [Export] Label text;
    [Export] TextureRect box;

    public override void _Ready()
    {
        instance = this;
    }

    public async void DisplayLine(string lineName)
    {
        Dialouge d = lines[lineName];
        switch (d.emotion)
        {
            case Dialouge.Emotion.SHOCKED:
                box.Texture = shockedTextBox;
                break;
            case Dialouge.Emotion.RELIEVED:
                box.Texture = relievedTextBox;
                break;
            case Dialouge.Emotion.AFRAID:
                box.Texture = afraidTextBox;
                break;
        }
        var tweenIn = CreateTween();
        tweenIn.SetEase(Tween.EaseType.Out);
        tweenIn.SetTrans(Tween.TransitionType.Cubic);
        tweenIn.TweenProperty(display, "modulate:a", 1.0, 0.2).From(0.0);
        await ToSignal(tweenIn, Tween.SignalName.Finished);
        text.Text = "";
        string full = d.line ?? "";
        for (int i = 0; i < full.Length; i++)
        {
            text.Text += full[i];
            var timer = GetTree().CreateTimer(Mathf.Max(0f, 0.03));
            await ToSignal(timer, SceneTreeTimer.SignalName.Timeout);
        }
        var holdTimer = GetTree().CreateTimer(3);
        await ToSignal(holdTimer, SceneTreeTimer.SignalName.Timeout);
        var tweenOut = CreateTween();
        tweenOut.SetEase(Tween.EaseType.In);
        tweenOut.SetTrans(Tween.TransitionType.Cubic);
        tweenOut.TweenProperty(display, "modulate:a", 0.0, 0.2);
        await ToSignal(tweenOut, Tween.SignalName.Finished);

    }


}
