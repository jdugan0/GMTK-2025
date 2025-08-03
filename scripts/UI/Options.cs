using Godot;
using System;

public partial class Options : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export] Button hardCoreButton;
	[Export] Button timer;
	[Export] Slider volume;
	public override void _Ready()
	{
		hardCoreButton.ButtonPressed = GameManager.instance.hardCore;
		timer.ButtonPressed = GameManager.instance.timer;
		volume.Value = AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("Master"));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetHardCore(bool toggle)
	{
		GameManager.instance.hardCore = toggle;
	}
	public void SetTimer(bool toggle)
	{
		GameManager.instance.timer = toggle;
	}

	public void AdjustVolume(float value)
	{
		AudioServer.SetBusVolumeLinear(AudioServer.GetBusIndex("Master"), value);
	}

	public async void MainMenu()
	{
		GameManager.instance.SaveProgress();
		await SceneSwitcher.instance.SwitchSceneAsyncSlide("MainMenu");
	}
}
