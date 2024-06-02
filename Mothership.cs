using System;
using Godot;

public partial class Mothership : Area2D
{
	[Signal]
	public delegate void UpdateScoreEventHandler(int points);

	[Export]
	private int Speed = 150;

	[Export]
	public PackedScene LasersScene { get; set; }

	[Export]
	public PackedScene ExplosionScene { get; set; }

	PathFollow2D Pathing { get; set; }
	private Timer _explosionTimer;
	private int lasersVelocity = 400;

	private void TriggerExplosion()
	{
		Node2D explosionScene = (Node2D)ExplosionScene.Instantiate();
		AddChild(explosionScene);
		CpuParticles2D explosion = GetNode<CpuParticles2D>("Explosion/CPUParticles2D");
		explosion.ScaleAmountMin = explosion.ScaleAmountMin * 2;
		explosion.ScaleAmountMax = explosion.ScaleAmountMax * 2;
		explosion.Amount = 40;
		explosion.Emitting = true;
		_explosionTimer.Start();
	}

	private void OnExplosionTimerTimeout()
	{
		CpuParticles2D explosion = GetNode<CpuParticles2D>("Explosion/CPUParticles2D");
		GD.Print("Are we still emitting? ", explosion.Emitting);
		if (!explosion.Emitting)
		{
			// Remove Mothership from screen after particles are done
			QueueFree();
		}
	}

	private void OnBodyEntered(Node2D body)
	{
		GetNode<Timer>("/root/Main/MothershipSpawnTimer").Start();
		EmitSignal(SignalName.UpdateScore, 50);
		body.QueueFree();
		TriggerExplosion();
		// QueueFree();
	}

	private void OnShootTimerTimeout()
	{
		// y: 36 x: -2 offset for having lasers come out of the eyes
		RigidBody2D lasers = LasersScene.Instantiate<RigidBody2D>();
		lasers.Position = new Vector2(-2, 36);
		lasers.LinearVelocity = new Vector2(0, lasersVelocity);
		AddChild(lasers);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pathing = (PathFollow2D)GetParent();
		GetNode<Timer>("ShootTimer").Start();
		UpdateScore += GetNode<Main>("/root/Main").UpdateScore;
		_explosionTimer = GetNode<Timer>("ExplosionTimer");
		_explosionTimer.Timeout += OnExplosionTimerTimeout;
	}

	// Called when the node is about to exit the scene tree
    public override void _ExitTree()
    {
		UpdateScore -= GetNode<Main>("/root/Main").UpdateScore;
		_explosionTimer.Timeout -= OnExplosionTimerTimeout;
        base._ExitTree();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		Pathing.Progress += Speed * (float)delta;
	}
}
