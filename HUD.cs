using Godot;

public partial class HUD : CanvasLayer
{
	[Signal]
    public delegate void StartGameEventHandler();

	public void UpdateHealth(int health)
	{
		GetNode<Label>("HealthLabel").Text = "Health: " + health.ToString();
	}

	public void UpdateScore(int score)
	{
		GetNode<Label>("ScoreLabel").Text = "Score: " + score.ToString();
	}

	private void OnStartButtonPressed()
	{
		GetNode<Button>("StartButton").Hide();
		GetNode<Label>("ControlsLabel").Hide();
		EmitSignal(SignalName.StartGame);
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
