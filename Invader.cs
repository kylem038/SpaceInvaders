using Godot;

public partial class Invader : Area2D
{
	private int Speed = 250;
	public string Direction { get; set; } = "right";
	private string NextDirection { get; set; } = "left";
	private bool HasTriggeredDownMovement = false;
	private bool KickOut = false;
	private float MoveCooldown = 0.5f;
	private float _timeSinceLastMove = 0f;
	private int Called = 0;

	private void OnBodyEntered(Node2D body)
	{
		GD.Print("Invader Hit!");
		// Update score?
		// Remove projectile from screen
		body.QueueFree();
		// Remove Invader from screen
		
		QueueFree();
	}

	private void ChangeDirection()
	{	

		// Move downward for 1 "game loop"
		if (!HasTriggeredDownMovement) {
			GD.Print("Starting down movement");
			Direction = "down";
			HasTriggeredDownMovement = true;
		}
		// Then continue back to other boundary
		else
		{
			GD.Print("Starting direction swap");
			Direction = NextDirection;
			if (NextDirection == "left")
			{
				NextDirection = "right";
			}
			else
			{
				NextDirection = "left";
			}

			// Reenable Boundary Collision Boxes
			GetNode<CollisionShape2D>("/root/Main/RightInvaderBoundary/CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
			GetNode<CollisionShape2D>("/root/Main/LeftInvaderBoundary/CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, false);

			HasTriggeredDownMovement = false;
			KickOut = true;
		}
		GD.Print("Current direction is: ", Direction);
		GD.Print("Next direction is: ", NextDirection);
		if (KickOut) return;
		Called++;
		GD.Print("Called: ", Called);
		ChangeDirection();
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
			if (Direction == "down")
			{
				velocity.Y += 1;
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
