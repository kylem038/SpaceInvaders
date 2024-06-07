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

	private Timer _explosionTimer { get; set; }

	private float ShootCooldown = 1.0f;
	private float _timeSinceLastShot = 0f;
	private int projectileYVelocity = -400;

	Texture2D projectileTexture = (Texture2D)ResourceLoader.Load("res://art/SpaceInvader-Projectile.png");

	private void TriggerExplosion()
	{
		CpuParticles2D explosion = GetNode<CpuParticles2D>("Explosion/CPUParticles2D");
		explosion.Amount = 30;
		explosion.Emitting = true;

		GetNode<AudioStreamPlayer>("/root/Main/Explosion").Play();
	}

	private void OnBodyEntered(Node2D body)
	{
		TriggerExplosion();
		GetNode<AnimationPlayer>("HitAnimationPlayer").Play("hit");
		EmitSignal(SignalName.Hit);
		body.QueueFree();
	}

	private void OnExplosionGameOverTimerTimeout()
	{
		CpuParticles2D explosion = GetNode<CpuParticles2D>("Explosion/CPUParticles2D");
		if (!explosion.Emitting)
		{
			// Remove Player from screen after particles are done
			QueueFree();
		}
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
		projectileInstance.Position = new Vector2(Position.X, Position.Y - 30);

		GetParent().AddChild(projectileInstance);

		// Set the texture
		projectileInstance.SetTexture(projectileTexture);

		// Set the velocity
		projectileInstance.SetVelocity(new Vector2(0, projectileYVelocity));

		// Play shoot sound
		AudioStreamPlayer shootSound = GetNode<AudioStreamPlayer>("/root/Main/Projectile");
		shootSound.PitchScale = (float)GD.RandRange(1, 1.5);
		shootSound.Play();
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		_explosionTimer = GetNode<Timer>("ExplosionTimer");
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
			// TODO: Start animation once we have those assets
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
