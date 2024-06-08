using Godot;

public partial class HUD : CanvasLayer
{
	[Signal]
	public delegate void StartGameEventHandler();

	public void SetMessage(string text)
	{
		GetNode<Label>("Message").Text = text;
	}

	public void UpdateHealth(int health)
	{
		GetNode<Label>("HealthLabel").Text = "Health: " + health.ToString();
	}

	public void UpdateScore(int score)
	{
		GetNode<Label>("ScoreLabel").Text = "Score: " + score.ToString();
	}

	public void TransitionToLevel()
	{
		GetNode<Label>("Message").Show();
		GetNode<Timer>("MessageTimer").Start();
	}

	private void OnStartButtonPressed()
	{
		GetNode<Button>("StartButton").Hide();
		GetNode<Label>("ControlsLabel").Hide();
		GetNode<Label>("Message").Text = "Level 1";
		TransitionToLevel();
	}

	public void OnMessageTimerTimeout()
	{
		EmitSignal(SignalName.StartGame);
	}

	public void OnGameOverTimerTimeout()
	{
		GetNode<Label>("Message").Hide();
		GetNode<Button>("StartButton").Show();
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
