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

	private int playerHealth = 3;
	private int score = 0;

	private int rowStart = 50;
	private int rowGutter = 50;
	private int numberOfRows = 4;
	private int columnStart = 150;
	private int columnGutter = 50;
	private int numberofColumns = 8;

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
				invaderPath.AddChild(invader);
				// Add Pathing to main scene
				AddChild(pathing);
			}
			
		}
	}

	private void SpawnMothership()
	{
		GD.Print("Spawning Mothership");
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
		Invader randomInvader = getRandomInvaderInstance();
		randomInvader.Shoot();
	}

	private void OnMothershipSpawnTimerTimeout()
	{
		SpawnMothership();
	}

	private void OnHudStartGame()
	{
		StartLevel();
	}

	private void StartLevel()
	{
		SpawnInvaders();

		// Start invader shoot timer
		GetNode<Timer>("InvaderShootTimer").Start();

		// Start mothership spawn timer (wait X secs)
		GetNode<Timer>("MothershipSpawnTimer").Start();
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
