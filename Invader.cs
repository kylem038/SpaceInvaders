using Godot;

public partial class Invader : Area2D
{
	[Export]
	public PackedScene ProjectileScene;

	[Export]
	private int Speed = 300;

	private float MoveCooldown = 0.5f;
	private float _timeSinceLastMove = 0f;
	private int projectileYVelocity = 300;
	Texture2D projectileTexture = (Texture2D)ResourceLoader.Load("res://art/SpaceInvader-Projectile.png");
	PathFollow2D Pathing { get; set; }

	private void OnBodyEntered(Node2D body)
	{
		GD.Print("Invader Hit!");
		// Update score?
		// Remove projectile from screen
		body.QueueFree();
		// Remove Invader from screen
		
		QueueFree();
	}

	public void Shoot()
	{
		// Need instance of Projectile
		Projectile projectileInstance = (Projectile)ProjectileScene.Instantiate();
		projectileInstance.Position = new Vector2(Position.X, Position.Y - 30);

		GetParent().AddChild(projectileInstance);

		// Set the texture
		projectileInstance.SetTexture(projectileTexture);

		// Set the velocity
		projectileInstance.SetVelocity(new Vector2(0, projectileYVelocity));
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pathing = (PathFollow2D)GetParent();
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
