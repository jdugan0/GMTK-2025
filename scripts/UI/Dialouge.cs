using Godot;
[GlobalClass]
public partial class Dialouge : Resource
{
    [Export] public string line;
    public enum Emotion
    {
        SHOCKED,
        RELIEVED,
        AFRAID
    }
    [Export] public Emotion emotion;
}