using Godot;
using System;

public partial class Options : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void AdjustVolume(float value)
	{
		AudioServer.SetBusVolumeLinear(AudioServer.GetBusIndex("Master"), value);
	}

	    public async void MainMenu()
    {
        await SceneSwitcher.instance.SwitchSceneAsyncSlide("MainMenu");
    }
}
