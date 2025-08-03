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
        if (Input.IsActionJustPressed("FIRE") && nextLevel)
        {
            GameManager.instance.NextLevel();
        }
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
