using Godot;

public partial class Player : Area2D
{
	[Export]
	public int Speed { get; set; } = 400; // How fast the player will move (pixels/sec).

	[Export]
	public PackedScene ProjectileScene;

	[Signal]
	public delegate void HitEventHandler();

	public Vector2 ScreenSize; // Size of the game window.

	private float ShootCooldown = 1.0f;
	private float _timeSinceLastShot = 0f;
	

	private void OnBodyEntered(Node2D body)
	{
		GD.Print("Player Hit!");
		// TODO: Trigger way to flash Player sprite
		// TODO: Remove Health
		EmitSignal(SignalName.Hit);
		GetNode<CollisionPolygon2D>("CollisionPolygon2D").SetDeferred(CollisionPolygon2D.PropertyName.Disabled, true);
	}

	public void Start(Vector2 position)
	{
		Position = position;
		GetNode<CollisionPolygon2D>("CollisionPolygon2D").Disabled = false;
	}

	private void Shoot()
	{
		// Need instance of Projectile
		Projectile projectileInstance = (Projectile)ProjectileScene.Instantiate();
		projectileInstance.Position = new Vector2(Position.X, Position.Y - 20);
		GetParent().AddChild(projectileInstance);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_timeSinceLastShot += (float)delta;
		Vector2 velocity = Vector2.Zero;

		if (Input.IsActionPressed("player_left"))
		{
			velocity.X -= 1;
		}
		if (Input.IsActionPressed("player_right"))
		{
			velocity.X += 1;
		}
		if (Input.IsActionPressed("player_shoot") && _timeSinceLastShot >= ShootCooldown)
		{
			Shoot();
			_timeSinceLastShot = 0;
		}

		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
			// Start animation once we have those assets
		}
		else 
		{
			// Stop animation
		}

		Position += velocity * (float)delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Position.Y
		);
	}
}
