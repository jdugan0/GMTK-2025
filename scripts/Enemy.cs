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
    [Export] float stopBuffer = 0f;
    [Export] float coolDown;
    float coolDownTimer;
    [Export] PackedScene enemyBullet;
    [Export] AnimatedSprite2D sprite;
    [Export] float fireSpeed = 8000.0f;
    [Export] Node2D firePos;
    bool playerIn = false;
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
        coolDownTimer += (float)delta;
        attackTimer += (float)delta;
        if (coolDownTimer >= coolDown)
        {
            MoveToPlayer();
        }

        if (GameManager.instance.player.GlobalPosition.DistanceTo(GlobalPosition) <= (desiredDistance + stopBuffer) && attackTimer >= attackDelay)
        {
            Attack();
        }
        CheckForSeenPlayer();
    }
    public async void Attack()
    {
        attackTimer = 0f;
        coolDownTimer = 0f;
        switch (enemyType)
        {
            case EnemyTypes.NORMAL:
                sprite.Play("Attack");
                if (playerIn)
                {
                    GameManager.instance.player.TakeDamage();
                }
                break;
            case EnemyTypes.RANGED:
                sprite.Play("Attack");
                await ToSignal(sprite, AnimatedSprite2D.SignalName.AnimationFinished);
                Node node = enemyBullet.Instantiate();
                GetTree().CurrentScene.AddChild(node);
                EnemyBullet b = (EnemyBullet)node;
                float angle = (GameManager.instance.player.GlobalPosition - firePos.GlobalPosition).Angle() + Mathf.Pi / 2;
                b.Velocity = fireSpeed * Vector2.Up.Rotated(angle) + Velocity;
                b.Rotate(angle);
                b.GlobalPosition = firePos.GlobalPosition;
                sprite.Play("Normal");
                break;
        }
    }
    public override void _Ready()
    {
        coolDownTimer = coolDown;
        attackTimer = attackDelay;
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
                SeenPlayer();
            }
        }
    }
    public async void SeenPlayer()
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
                await ToSignal(GetTree().CreateTimer(0.4f), "timeout");
                node.SeenPlayer();
                break;
            }
        }
    }
    public void MoveToPlayer()
    {
        if (!playerSeen) return;

        Vector2 playerPos = GameManager.instance.player.GlobalPosition;
        bool playerCurrentlySeen = false;
        {
            var spaceState = GetWorld2D().DirectSpaceState;
            var query = PhysicsRayQueryParameters2D.Create(GlobalPosition, playerPos);
            query.Exclude = new Godot.Collections.Array<Rid> { GetRid() };

            var result = spaceState.IntersectRay(query);
            if (result.Count > 0 && (Node2D)result["collider"] is Movement)
                playerCurrentlySeen = true;
        }
        float dist = GlobalPosition.DistanceTo(playerPos);
        float innerBand = desiredDistance - stopBuffer;
        float outerBand = desiredDistance + stopBuffer;

        if (!playerCurrentlySeen || dist > outerBand)
        {
            nav.TargetPosition = playerPos;
            if (!nav.IsTargetReached())
            {
                Vector2 dir = ToLocal(nav.GetNextPathPosition()).Normalized();
                Velocity = dir * maxSpeed;
            }
            else
            {
                Velocity = Vector2.Zero;
            }
        }
        else if (dist < innerBand)
        {
            Vector2 dir = (GlobalPosition - playerPos).Normalized();
            Vector2 rawTarget = playerPos + dir * desiredDistance;
            Rid navMap = nav.GetNavigationMap();
            Vector2 safeTarget = NavigationServer2D.MapGetClosestPoint(navMap, rawTarget);
            nav.TargetPosition = safeTarget;
            if (!nav.IsTargetReached())
            {
                Vector2 dirV = ToLocal(nav.GetNextPathPosition()).Normalized();
                Velocity = dirV * maxSpeed;
            }
            else
            {
                Velocity = Vector2.Zero;
            }
        }
        else
        {
            Velocity = Vector2.Zero;
        }

        MoveAndSlide();
    }

    public void BodyEntered(Node2D node)
    {
        if (node is Movement m)
        {
            playerIn = true;
        }
    }
    public void BodyExited(Node2D node)
    {
        if (node is Movement m)
        {
            playerIn = false;
        }
    }
}
