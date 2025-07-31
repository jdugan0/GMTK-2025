using Godot;
using System;

public partial class EnemiesRemaining : Label
{
    int originalCount = 0;
    bool playedSound = false;   
    public override void _Process(double delta)
    {
        if (originalCount == 0)
        {
            originalCount = GetTree().GetNodesInGroup("Enemy").Count;
        }
        Text = GetTree().GetNodesInGroup("Enemy").Count + " / " + originalCount;
        GameManager.instance.enemiesRemaining = GetTree().GetNodesInGroup("Enemy").Count;
        if (GameManager.instance.enemiesRemaining == 0 && !playedSound)
        {
            playedSound = true;
            AudioManager.instance.PlaySFX("killedAll");
        }
    }

}
