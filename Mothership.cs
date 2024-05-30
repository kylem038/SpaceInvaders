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
	PathFollow2D Pathing { get; set; }
	private int lasersVelocity = 400;

	private void OnBodyEntered(Node2D body)
	{
		GetNode<Timer>("/root/Main/MothershipSpawnTimer").Start();
		EmitSignal(SignalName.UpdateScore, 50);
		body.QueueFree();
		QueueFree();
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
	}

	// Called when the node is about to exit the scene tree
    public override void _ExitTree()
    {
		UpdateScore -= GetNode<Main>("/root/Main").UpdateScore;
        base._ExitTree();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		Pathing.Progress += Speed * (float)delta;
	}
}
