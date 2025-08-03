using Godot;
using System;

public partial class Collectible : Node2D
{
	[Export] public int id;
	[Export] public Texture2D image;
	[Export] public float scale = 1;

	private bool onScreen = false;
	[Export] private TextureRect textureRect;
	[Export] private Label label;
	[Export] CanvasLayer parent;
	public override void _Ready()
	{
		textureRect.Texture = image;
		label.Text = "Collectible # " + id;
	}



	public void ResolvePickup(Node2D hit)
	{
		if (hit is Movement player)
		{
			if (player.isRolling) return;
			// AudioManager.instance.PlaySFX("foundCollectible");
			parent.Show();
			onScreen = true;
			GetTree().Paused = true;
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (onScreen && @event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			parent.Hide();
			onScreen = false;
			GetTree().Paused = false;
			QueueFree();
		}
	}
}
