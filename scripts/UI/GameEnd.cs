using Godot;
using System;
using System.Threading.Tasks;

public partial class GameEnd : Node
{
    [Export] public float time = 5f;
    float timer = 0;
    public override async void _Process(double delta)
    {
        timer += (float)delta;
        if (timer > time)
        {
            timer = 0;
            await SceneSwitcher.instance.SwitchSceneAsyncSlide("GameComplete2");
        }
    }

}
