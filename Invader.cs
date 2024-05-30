using Godot;

public partial class Invader : Area2D
{
	[Signal]
	public delegate void UpdateScoreEventHandler(int points);

	[Export]
	public PackedScene ProjectileScene;

	[Export]
	private int Speed = 300;

	private float MoveCooldown = 0.5f;
	private float _timeSinceLastMove = 0f;
	private int projectileYVelocity = 150;
	Texture2D projectileTexture = (Texture2D)ResourceLoader.Load("res://art/Invader-Bomb.png");
	PathFollow2D Pathing { get; set; }

	private void OnBodyEntered(Node2D body)
	{
		// Update score?
		EmitSignal(SignalName.UpdateScore, 10);
		// Remove projectile from screen
		body.QueueFree();
		// Remove Invader from screen
		QueueFree();
	}

	public void Shoot()
	{
		// Need instance of Projectile
		Projectile projectileInstance = (Projectile)ProjectileScene.Instantiate();
		projectileInstance.Position = new Vector2(Position.X, Position.Y);

		GetParent().AddChild(projectileInstance);

		// Set the texture
		projectileInstance.SetTexture(projectileTexture);

		// Set the velocity
		projectileInstance.SetVelocity(new Vector2(0, projectileYVelocity));

		// Set collision layer and mask
		projectileInstance.CollisionLayer = 1 << 3;
		projectileInstance.CollisionMask = (1 << 0) | (1 << 2);
		
		// Set collision for Area2D child to detect collision with Player projectile
		Area2D childCollision = projectileInstance.GetNode<Area2D>("Area2D");
		childCollision.CollisionLayer = 1 << 4;
		childCollision.CollisionMask = 1 << 2;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pathing = (PathFollow2D)GetParent();
		UpdateScore += GetNode<Main>("/root/Main").UpdateScore;
	}

    public override void _ExitTree()
    {
		UpdateScore -= GetNode<Main>("/root/Main").UpdateScore;
        base._ExitTree();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		// Make the Invaders appear to move on a timer
		_timeSinceLastMove += (float)delta;
		if (_timeSinceLastMove > MoveCooldown)
		{
			Pathing.Progress += Speed * (float)delta;
			_timeSinceLastMove = 0;
		}
	}
}
