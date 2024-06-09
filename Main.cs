using Godot;
using System;
using System.Collections.Generic;

public partial class Main : Node2D
{
	[Export]
	public PackedScene InvaderScene { get; set; }

	[Export]
	public PackedScene PathingScene { get; set; }

	[Export]
	public PackedScene PlayerScene { get; set; }

	[Export]
	public PackedScene MothershipPathingScene { get; set; }

	[Export]
	public PackedScene BunkerScene { get; set; }

	private BunkerLocations bunkerLocations { get; set; }

	private int _playerHealth = 3;
	private int _score = 0;
	private bool roundInProgress = false;

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
			return 500;
		}
		else
		{
			return 700;
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

	public class BunkerLocations 
	{
		public Vector2[] level1 = { 
			new Vector2(320, 384),
			new Vector2(192, 384),
			new Vector2(448, 384)
		};
		public Vector2[] level2 = {
			new Vector2(192, 384),
			new Vector2(448, 384)
		};

		public Vector2[] level3 = {
			new Vector2(256, 384),
			new Vector2(384, 384),
			new Vector2(192, 384),
			new Vector2(448, 384)
		};
	}

	private void SpawnBunkers()
	{
		GetTree().CallGroup("bunkers", Node.MethodName.QueueFree);
		Vector2[] locations;


		if (currentLevel == 1)
		{
			// spawn 3 bunkers
			locations = bunkerLocations.level1;
		}
		else if (currentLevel == 2)
		{
			// spawn 2 bunkers at (192, 384) & (448, 384)
			locations = bunkerLocations.level2;
		}
		else if (currentLevel == 3)
		{
			// spawn 4 bunkers
			locations = bunkerLocations.level3;
		}
		else
		{
			locations = null;
			GD.Print("Warning: out of level range for bunkers");
		}

		for (int i = 0; i < locations.Length; i++)
		{
			Bunker bunker = BunkerScene.Instantiate<Bunker>();
			bunker.Position = locations[i];
			AddChild(bunker);
		}
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

	public void OnPlayerHit()
	{
		Timer playerExplosionTimer = GetNode<Timer>("Player/ExplosionTimer");
		_playerHealth--;
		GetNode<HUD>("HUD").UpdateHealth(_playerHealth);
		if (_playerHealth == 0)
		{
			playerExplosionTimer.Start();
			GameOver();
		}
	}

	public void UpdateScore(int points)
	{
		_score += points;
		GetNode<HUD>("HUD").UpdateScore(_score);
	}

	private void OnHudStartGame()
	{
		GD.Print("We're starting a new game");
		StartLevel();
	}

	private void TransitionFromGameOver()
	{
		// Reset hud stats
		_playerHealth = 3;
		GetNode<HUD>("HUD").UpdateHealth(_playerHealth);
		_score = 0;
		GetNode<HUD>("HUD").UpdateScore(_score);
		currentLevel = 1;

		// Reset shoot timer
		GetNode<Timer>("InvaderShootTimer").WaitTime = 3;

		// Add new player
		Player newPlayer = PlayerScene.Instantiate<Player>();
		Marker2D playerStartLocation = GetNode<Marker2D>("PlayerStart");
		newPlayer.Position = playerStartLocation.Position;
		newPlayer.Hit += OnPlayerHit;
		AddChild(newPlayer);

	}

	private void StartLevel()
	{
		roundInProgress = true;

		// Coming from a game over we need to see if Player is still in the scene
		// If not we need a new player
		Player player = GetNodeOrNull<Player>("Player");
		if (player == null)
		{
			TransitionFromGameOver();
		}

		SpawnInvaders();
		SpawnBunkers();

		// Start invader shoot timer
		GetNode<Timer>("InvaderShootTimer").Start();

		// Start mothership spawn timer (wait X secs)
		GetNode<Timer>("MothershipSpawnTimer").Start();

		// Hide Level 1 Message
		GetNode<Label>("HUD/Message").Hide();

		// Check if music is playing, if not start it up
		// Its set to loop so just need this once
		AudioStreamPlayer music = GetNode<AudioStreamPlayer>("Music");
		if (!music.Playing)
		{
			music.Play();
		}
	}

	private void GameOver()
	{
		// Clean up invader & mothership instances
		GetTree().CallGroup("invaders", Node.MethodName.QueueFree);
		GetTree().CallGroup("bunkers", Node.MethodName.QueueFree);

		if (GetTree().GetNodesInGroup("mothership").Count != 0)
		{
			GetTree().CallGroup("mothership", Node.MethodName.QueueFree);
		}
		else
		{
			// Stop mothership timer
			GetNode<Timer>("MothershipSpawnTimer").Stop();
		}


		GetNode<HUD>("HUD").SetMessage("Game Over");
		GetNode<Label>("HUD/Message").Show();
		GetNode<Timer>("HUD/GameOverTimer").Start();

		roundInProgress = false;
	}

	private void MoveToNextLevel()
	{
		currentLevel++;

		if (currentLevel < 4)
		{
			roundInProgress = false;
			GetNode<HUD>("HUD").SetMessage($"Level {currentLevel}");
			GetNode<HUD>("HUD").TransitionToLevel();
			GetNode<Timer>("InvaderShootTimer").WaitTime -= 1;
		}
		else
		{
			// Show Thank you message
			roundInProgress = false;
			GetNode<HUD>("HUD").SetMessage("Victory! Thank You For Playing My Game!");
			GetNode<Label>("HUD/Message").Show();
			GetNode<Timer>("MothershipSpawnTimer").Stop();
		}
	}

	private void CheckEnemies()
	{
		if (GetTree().GetNodesInGroup("invaders").Count == 0
			&& GetTree().GetNodesInGroup("mothership").Count == 0
			&& roundInProgress
			&& _playerHealth != 0)
		{
			GD.Print("All enemies have been removed from the scene.");
			MoveToNextLevel();
		}
	}


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		bunkerLocations = new BunkerLocations();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (roundInProgress)
		{
			CheckEnemies();
		}
		
	}
}
