using Godot;
using System;

public partial class GameManager : Node
{
    public Movement player;
    public static GameManager instance;
    public override void _Ready()
    {
        instance = this;
    }

}
