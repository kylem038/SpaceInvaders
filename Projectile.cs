using Godot;

public partial class Projectile : RigidBody2D
{
	[Export]
	public Vector2 Velocity = new Vector2(0, -400);

	private void OnBodyEntered(Node2D body)
	{
		GD.Print("Projectile Hit!", body);
		if (body is Area2D area)
		{
			if (area.IsInGroup("invaders"))
			{
				QueueFree();
			}
			
		}
		
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Set the initial velocity
        LinearVelocity = Velocity;
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
