using Godot;
using System;

public partial class BulletCount : Node
{
    [Export] PackedScene bulletIcon;
    private int prevCount = -1;

    public override void _Process(double delta)
    {
        int bulletCount = GameManager.instance.player.bullets;
        if (bulletCount != prevCount)
            UpdateIcons(bulletCount);
    }
    private void UpdateIcons(int bulletCount)
    {

        int current = GetChildCount();
        for (int i = current - 1; i >= bulletCount; i--)
        {
            Node extra = GetChild(i);
            RemoveChild(extra);
            extra.QueueFree();
        }
        for (int i = current; i < bulletCount; i++)
        {
            Node icon = bulletIcon.Instantiate();
            AddChild(icon);
        }

        prevCount = bulletCount;
    }
}
