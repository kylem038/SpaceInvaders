using Godot;

public partial class Invader : Area2D
{
	private int Speed = 250;
	public string Direction { get; set; } = "right";
	private float MoveCooldown = 0.5f;
	private float _timeSinceLastMove = 0f;

	private void OnBodyEntered(Node2D body)
	{
		GD.Print("Invader Hit!");
		// Update score?
		// Remove projectile from screen
		body.QueueFree();
		// Remove Invader from screen
		
		QueueFree();
	}

	private void ChangeDirection(string direction)
	{
		Direction = direction;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Make the Invaders appear to move on a timer
		// To mimic original space invaders
		_timeSinceLastMove += (float)delta;
		Vector2 velocity = Vector2.Zero;

		if (_timeSinceLastMove > MoveCooldown)
		{
			if (Direction == "right")
			{
				velocity.X += 1;
			} 
			if (Direction == "left")
			{
				velocity.X -= 1;
			}

			if (velocity.Length() > 0)
			{
				velocity = velocity.Normalized() * Speed;
			}

			Position += velocity * (float)delta;

			_timeSinceLastMove = 0;
		}
	}
}
