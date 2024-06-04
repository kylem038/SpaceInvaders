using Godot;
using System;

public partial class Bunker : Area2D
{
	private int _health = 3;

	AnimatedSprite2D animation { get; set; }

	private void TriggerExplosion()
	{
		CpuParticles2D explosion = GetNode<CpuParticles2D>("Explosion/CPUParticles2D");
		explosion.Emitting = true;

		GetNode<AudioStreamPlayer>("/root/Main/Explosion").Play();
	}

	private void OnBodyEntered(Node2D body)
	{
		TriggerExplosion();
		_health -= 1;
		if (_health == 0)
		{
			QueueFree();
		}
		else
		{
			animation.Frame += 1;
		}
		
		body.QueueFree();
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animation.Frame = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
