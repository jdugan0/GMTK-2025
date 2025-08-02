using Godot;
using System;

public partial class SpeakTrigger : Area2D
{
    bool fired;
    [Export] string lineName;
    [Export] TriggerType type;


    public enum TriggerType { ENTERED, START, FIRED, ENEMYHIT, DAMAGE, ENDED }

    [Export] public bool onlyOnEnd;

    public override void _Ready()
    {
        CallDeferred(nameof(SetupTriggers));
    }
    public override void _Process(double delta)
    {
        if (type == TriggerType.ENDED && GameManager.instance.enemiesRemaining == 0 && !fired)
        {
            Trigger();
        }
    }

    public void SetupTriggers()
    {
        switch (type)
        {
            case TriggerType.START:
                // Defer to ensure scene finished instancing
                CallDeferred(nameof(Trigger));
                break;

            case TriggerType.FIRED:
                GameManager.instance.player.Fired += OnPlayerFired;
                break;

            case TriggerType.DAMAGE:
                GameManager.instance.player.Damaged += OnPlayerDamaged;
                break;

            case TriggerType.ENEMYHIT:
                GameManager.instance.EnemyHit += OnEnemyHit;
                break;
        }
    }

    private void OnBodyEntered(Node body)
    {
        if (fired) return;
        if (body is Movement && type == TriggerType.ENTERED)
            Trigger();
    }

    private void OnPlayerFired() => Trigger();
    private void OnPlayerDamaged(int amount) => Trigger();
    private void OnEnemyHit() => Trigger();

    private void Trigger()
    {
        if (onlyOnEnd)
        {
            GD.Print(1);
        }
        if (onlyOnEnd && GameManager.instance.enemiesRemaining != 0) return;
        if (onlyOnEnd)
        {
            GD.Print(2);
        }
        if (fired) return;
        if (onlyOnEnd)
        {
            GD.Print(3);
        }
        DialogueManager.instance.EnqueueLine(lineName);
        fired = true;
    }
}
