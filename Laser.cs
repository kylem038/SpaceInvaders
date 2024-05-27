using Godot;
using System;

public partial class Laser : RigidBody2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Handle boundaries
		if (Position.Y < 0 || Position.Y > GetViewportRect().Size.Y)
		{
			QueueFree();
		}
	}
}
