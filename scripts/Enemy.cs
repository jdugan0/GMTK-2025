using Godot;
using System;
using System.Diagnostics.Tracing;

public partial class Enemy : CharacterBody2D
{
    [Export] NavigationAgent2D nav;
    [Export] float maxSpeed;
    [Export] float playerSeenDistance;
    [Export] float alertDistance;
    bool playerSeen;

    public void Death()
    {
        AudioManager.instance.PlaySFX("hitSuccess");
        QueueFree();
    }
    public override void _PhysicsProcess(double delta)
    {
        if (!IsInstanceValid(GameManager.instance.player)) return;
        MoveToPlayer();
        CheckForSeenPlayer();
        
        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            var c = GetSlideCollision(i);
            OnCol(c.GetCollider() as Node2D);
        }
    }
    public void CheckForSeenPlayer()
    {
        var spaceState = GetWorld2D().DirectSpaceState;
        var query = PhysicsRayQueryParameters2D.Create(GlobalPosition, GameManager.instance.player.GlobalPosition);
        query.Exclude = new Godot.Collections.Array<Rid> { GetRid() };
        var result = spaceState.IntersectRay(query);
        if (result.Count > 0)                       // something was hit
        {
            var collider = (Node2D)result["collider"];
            if (collider is Movement && GlobalPosition.DistanceTo(GameManager.instance.player.GlobalPosition) < playerSeenDistance)
            {
                GD.Print(Name);
                SeenPlayer();
            }
        }
    }
    public void SeenPlayer()
    {
        playerSeen = true;
        foreach (Enemy node in GetTree().GetNodesInGroup("Enemy"))
        {
            if (node.GlobalPosition.DistanceTo(GlobalPosition) < alertDistance)
            {
                if (node.playerSeen)
                {
                    return;
                }
                node.SeenPlayer();
            }
        }
    }
    public void MoveToPlayer()
    {
        if (!playerSeen) return;
        nav.TargetPosition = GameManager.instance.player.GlobalPosition;
        if (!nav.IsTargetReached())
        {
            var dir = ToLocal(nav.GetNextPathPosition()).Normalized();
            Velocity = dir * maxSpeed;
            MoveAndSlide();
        }
    }

    public void OnCol(Node2D node)
    {
        if (node is Movement m)
        {
            m.Death();
        }
    }

}
