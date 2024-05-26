using Godot;

public partial class Mothership : Area2D
{
	private void OnBodyEntered(Node2D body)
	{
		GD.Print("Mothership got hit!");
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
