using Godot;
using System;

public partial class GameManager : Node
{
    public static GameManager instance;
    public BaseModule player;
    public override void _Ready()
    {
        instance = this;
    }

}
