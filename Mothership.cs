using Godot;

public partial class Mothership : Area2D
{
	[Export]
	private int Speed = 150;
	PathFollow2D Pathing { get; set; }
	private void OnBodyEntered(Node2D body)
	{
		// TODO: Score bonus points
		body.QueueFree();
		QueueFree();
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pathing = (PathFollow2D)GetParent();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Pathing.Progress += Speed * (float)delta;
	}
}
