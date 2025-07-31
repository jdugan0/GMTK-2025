using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
    [Export] NavigationAgent2D nav;
    [Export] PackedScene ghost;
    [Export] float maxSpeed;
    [Export] float playerSeenDistance;
    [Export] float alertDistance;
    bool playerSeen;
    bool playedSound = false;
    [Export] float attackDelay;
    float attackTimer;
    [Export] float desiredDistance = 0f;
    enum EnemyTypes
    {
        NORMAL,
        RANGED
    }
    [Export] EnemyTypes enemyType = EnemyTypes.NORMAL;

    public void Death()
    {
        AudioManager.instance.PlaySFX("hitSuccess");
        Node2D ghostNode = ghost.Instantiate<Node2D>();
        GetTree().CurrentScene.AddChild(ghostNode);
        ghostNode.GlobalPosition = GlobalPosition;
        QueueFree();
    }
    public override void _PhysicsProcess(double delta)
    {
        if (!IsInstanceValid(GameManager.instance.player)) return;
        MoveToPlayer();
        CheckForSeenPlayer();
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
                // GD.Print(Name);
                SeenPlayer();
            }
        }
    }
    public void SeenPlayer()
    {
        if (!playedSound)
        {
            AudioManager.instance.PlaySFX("playerSpotted");
            playedSound = true;
        }
        playerSeen = true;
        foreach (Enemy node in GetTree().GetNodesInGroup("Enemy"))
        {
            if (node.GlobalPosition.DistanceTo(GlobalPosition) < alertDistance)
            {
                if (node.playerSeen)
                {
                    continue;
                }
                node.SeenPlayer();
            }
        }
    }
    public void MoveToPlayer()
    {
        if (!playerSeen) return;
        var spaceState = GetWorld2D().DirectSpaceState;
        var query = PhysicsRayQueryParameters2D.Create(GlobalPosition, GameManager.instance.player.GlobalPosition);
        query.Exclude = new Godot.Collections.Array<Rid> { GetRid() };
        bool playerCurrentlySeen = false;
        var result = spaceState.IntersectRay(query);
        if (result.Count > 0)                       // something was hit
        {
            var collider = (Node2D)result["collider"];
            if (collider is Movement)
            {
                playerCurrentlySeen = true;
            }
        }
        if (!nav.IsTargetReached() && GlobalPosition.DistanceTo(GameManager.instance.player.GlobalPosition) > desiredDistance || !playerCurrentlySeen)
        {
            nav.TargetPosition = GameManager.instance.player.GlobalPosition;
            var dir = ToLocal(nav.GetNextPathPosition()).Normalized();
            Velocity = dir * maxSpeed;
        }
        else
        {
            Velocity = Vector2.Zero;

        }
        MoveAndSlide();
    }

    public void OnCol(Node2D node)
    {
        if (node is Movement m)
        {
            m.Death();
        }
    }

}
