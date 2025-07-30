using Godot;
using System;

public partial class ParticleManager : Node
{
	[Export] GpuParticles2D[] particleReferences;

	public static ParticleManager instance;
    public override void _Ready()
    {
        instance = this;
    }

    public void AddOneShotParticle(int id, Vector2 position, Node obj){
		GpuParticles2D g = (GpuParticles2D)particleReferences[id].Duplicate();
		g.OneShot = true;
		g.Position = position;
		obj.AddChild(g);
		g.Emitting = true;
		g.Connect("finished", new Callable(g, MethodName.QueueFree));
	}

	public GpuParticles2D GetParticleReference(int id){
		return particleReferences[id];
	}
}
