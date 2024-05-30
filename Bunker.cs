using Godot;
using System;

public partial class Bunker : Area2D
{
	private int _health = 4;

	private void OnBodyEntered(Node2D body)
	{
		body.QueueFree();
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
