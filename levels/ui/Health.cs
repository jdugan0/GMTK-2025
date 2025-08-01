using Godot;
using System;

public partial class Health : TextureRect
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// hard coded, but it's such a small thing it doesn't matter
	public void updateHealth(int health)
	{
		if (health == 2)
		{
			Texture = GD.Load<Texture2D>("res://art/UI/heart.png");
		}
		else if (health == 1)
		{
			Texture = GD.Load<Texture2D>("res://art/UI/half_heart.png");
		}
		else if (health <= 0)
		{
			Texture = GD.Load<Texture2D>("res://art/UI/empty_heart.png");
		}
	}
}
