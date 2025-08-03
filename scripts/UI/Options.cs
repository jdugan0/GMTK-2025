using Godot;
using System;

public partial class Options : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export] Button hardCoreButton;
	public override void _Ready()
	{
		hardCoreButton.ButtonPressed = GameManager.instance.hardCore;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetHardCore(bool toggle)
	{
		GameManager.instance.hardCore = toggle;
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
