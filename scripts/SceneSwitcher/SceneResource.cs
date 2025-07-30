using Godot;
using System;
[GlobalClass]
public partial class SceneResource : Resource
{
    [Export] public string sceneName;
    [Export] public PackedScene scene;
}
