using Godot;
using System;

public partial class Invader : Area2D
{
	private void OnBodyEntered(Node2D body)
	{
		GD.Print("Invader Hit!");
		// Update score?
		// Remove Invader from screen
		QueueFree();
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
