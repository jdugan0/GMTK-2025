using Godot;
[GlobalClass]
public partial class Sound : Resource
{
    [Export] public AudioStream stream { get; set; }
    [Export] public float volume { get; set; }
    [Export] public string name { get; set; }
}