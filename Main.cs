using Godot;
using System.Collections.Generic;

public partial class Main : Node2D
{
	[Export]
	public PackedScene InvaderScene { get; set; }

	[Export]
	public PackedScene PathingScene { get; set; }

	[Export]
	public PackedScene MothershipPathingScene { get; set; }

	private int _playerHealth = 3;
	private int _score = 0;
	private bool roundStarted = false;

	private int rowStart = 50;
	private int rowGutter = 50;
	private int numberOfRows = 4;
	private int columnStart = 150;
	private int columnGutter = 50;
	private int numberofColumns = 8;
	private int currentLevel = 1;

	private int[] generateColumns()
	{
		List<int> invaderColumns = new List<int>();
		for (int i = 0; i < numberofColumns; i++)
		{
			invaderColumns.Add(columnStart + (columnGutter * i));
		}
		return invaderColumns.ToArray();
	}

	private int[] generateRows()
	{
		List<int> invaderRows = new List<int>();
		for (int i = 0; i < numberOfRows; i++)
		{
			invaderRows.Add(rowStart + (rowGutter * i));
		}
		return invaderRows.ToArray();
	}

	private Vector2 getInvaderSpawnPosition(int column, int row)
	{
		return new Vector2(column, row);
	}

	private int GetInvaderSpeed()
	{
		if (currentLevel == 1)
		{
			return 300;
		} 
		else if (currentLevel == 2)
		{
			return 400;
		} 
		else
		{
			return 500;
		}
	}

	private void SpawnInvaders()
	{
		int[] invaderRows = generateRows();
		int[] invaderColumns = generateColumns();
		for (int i = 0; i < invaderRows.Length; i++)
		{
			foreach (int column in invaderColumns)
			{
				// Create instance of Pathing
				Path2D pathing = PathingScene.Instantiate<Path2D>();
				// Create starting point for Pathing
				pathing.Position = getInvaderSpawnPosition(column, invaderRows[i]);
				// Add instance of Invader as child of PathFollow2D
				PathFollow2D invaderPath = (PathFollow2D)pathing.GetChild(0);
				Invader invader = InvaderScene.Instantiate<Invader>();
				int invaderSpeed = GetInvaderSpeed();
				invader.Speed = invaderSpeed;
				invaderPath.AddChild(invader);
				// Add Pathing to main scene
				AddChild(pathing);
			}
		}
	}

	private void SpawnMothership()
	{
		Path2D pathing = MothershipPathingScene.Instantiate<Path2D>();
		pathing.Position = new Vector2(0, 64);
		AddChild(pathing);
	}

	private List<T> GetChildrenOfType<T>(Node parentNode) where T : Node
    {
        List<T> childrenOfType = new List<T>();

        foreach (Node child in parentNode.GetChildren())
        {
            if (child is T)
            {
                childrenOfType.Add(child as T);
            }

            // Recursively check for children of this child
            childrenOfType.AddRange(GetChildrenOfType<T>(child));
        }

        return childrenOfType;
    }

	private Invader getRandomInvaderInstance()
	{
		List<Invader> invaderInstances = GetChildrenOfType<Invader>(GetNode<Main>("/root/Main"));
		// pick random instance
		Invader randomInvader = invaderInstances[GD.RandRange(0, invaderInstances.Count - 1)];
		return randomInvader;
	}

	private void OnInvaderShootTimerTimeout()
	{
		var invaders = GetTree().GetNodesInGroup("invaders");
		if (invaders.Count > 0)
		{
			Invader randomInvader = getRandomInvaderInstance();
			randomInvader.Shoot();
		}
	}

	private void OnMothershipSpawnTimerTimeout()
	{
		SpawnMothership();
	}

	private void OnPlayerHit()
	{
		_playerHealth--;
		GetNode<HUD>("HUD").UpdateHealth(_playerHealth);
		if (_playerHealth == 0)
		{
			GD.Print("Implement game over");
		}
	}

	public void UpdateScore(int points)
	{
		_score += points;
		GetNode<HUD>("HUD").UpdateScore(_score);
	}

	private void OnHudStartGame()
	{
		GD.Print("STARTING LEVEL?");
		StartLevel();
	}

	private void StartLevel()
	{
		roundStarted = true;
		
		SpawnInvaders();

		// Start invader shoot timer
		GetNode<Timer>("InvaderShootTimer").Start();

		// Start mothership spawn timer (wait X secs)
		GetNode<Timer>("MothershipSpawnTimer").Start();

		// Hide Level 1 Message
		GetNode<Label>("HUD/Message").Hide();

		// Check if music is playing, if not start it up
		// Its set to loop so just need this once
		AudioStreamPlayer music = GetNode<AudioStreamPlayer>("Music");
		if(!music.Playing)
		{
			music.Play();
		}
	}

	// private void GameOver()
	// {

	// }

	private void CheckEnemies()
{
    if (GetTree().GetNodesInGroup("invaders").Count == 0 
		&& GetTree().GetNodesInGroup("mothership").Count == 0
		&& roundStarted)
    {
        GD.Print("All enemies have been removed from the scene.");
		currentLevel++;

		if (currentLevel < 4)
		{
			roundStarted = false;
			GetNode<HUD>("HUD").SetMessage($"Level {currentLevel}");
			GetNode<HUD>("HUD").TransitionToLevel();
		} 
		else 
		{
			// Show Thank you message
		}
    }
}


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		CheckEnemies();
	}
}
