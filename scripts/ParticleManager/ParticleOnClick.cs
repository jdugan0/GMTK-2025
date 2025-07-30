using Godot;
using System;
using System.Runtime.InteropServices;

public partial class ParticleOnClick : Node2D
{

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("Click")){
			ParticleManager.instance.AddOneShotParticle(0, GetGlobalMousePosition(), this);
		}
	}
	public void OnSliderChanged(float value){
		// GD.Print("Value: " + value + " gravity: " + ParticleManager.instance.getParticle(0).ProcessMaterial.Get("gravity"));
		ParticleManager.instance.GetParticleReference(0).ProcessMaterial.Set("gravity", new Vector3(0, value, 0));
	}
	public void OnSliderChangedLifetime(float value){
		// GD.Print(ParticleManager.instance.getParticle(0).ProcessMaterial.Get("lifetime"));
		ParticleManager.instance.GetParticleReference(0).Lifetime = value;
	}
}
