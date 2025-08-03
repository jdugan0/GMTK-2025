using Godot;
using System;
using System.Threading.Tasks;

public partial class GameEnd : Node
{
    [Export] public float time = 5f;
    [Export] public bool nextLevel = false;
    [Export] public string nextScene = "GameComplete2";
    float timer = 0;

    public override async void _Process(double delta)
    {
        timer += (float)delta;
        if (timer > time)
        {
            timer = 0;
            if (nextLevel)
            {
                GameManager.instance.NextLevel();
            }
            else
            {
                await SceneSwitcher.instance.SwitchSceneAsyncSlide(nextScene);
            }
        }
    }

}
