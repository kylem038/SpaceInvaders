using Godot;

public partial class Projectile : RigidBody2D
{
	private AnimatedSprite2D _animatedSprite;

	public void SetTexture(Texture2D texture)
    {
        var frames = new SpriteFrames();
        frames.AddFrame("default", texture);
        _animatedSprite.SpriteFrames = frames;
        _animatedSprite.Play("default");
    }

	public void SetVelocity(Vector2 velocity)
	{
		LinearVelocity = velocity;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Set sprite
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
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
