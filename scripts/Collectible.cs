using Godot;
using System;

public partial class Collectible : Node2D
{
	[Export] public int id;
	[Export] public Texture2D image;
	[Export] public float scale = 1;

	private bool onScreen = false;
	private TextureRect textureRect;
	private Label label;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		textureRect = GetNode<TextureRect>("Content/Control/TextureRect");
		textureRect.Texture = image;
		textureRect.Scale = new Vector2(scale, scale);
		GetNode<Label>("Content/Control/Label").Text = "Collectible # " + id;
		label = GetNode<Label>("Content/Control/Label");
	}



	public void ResolvePickup(Node2D hit)
	{
		if (hit is Movement player)
		{
			if (player.isRolling) return;
			AudioManager.instance.PlaySFX("foundCollectible");
			textureRect.Show();
			label.Visible = true;
			onScreen = true;
			GetTree().Paused = true;
		}
	}
	
	    public override void _Input(InputEvent @event)
    {
		if (onScreen && @event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			textureRect.Hide();
			label.Visible = false;
			onScreen = false;
			GetTree().Paused = false;
			QueueFree();
        }
    }
}
