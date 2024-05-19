using Godot;
using System;

public partial class Main : Node2D
{
	[Export]
	public PackedScene InvaderScene { get; set; }

	private int[] invaderColumns = { 150, 250, 350 };
	private int[] invaderRows = { 250, 150, 50 };

	private Vector2 getInvaderSpawnPosition(int column, int row)
	{
		return new Vector2(column, row);
	}

	private void SpawnInvaders()
	{
		for (int i = 0; i < invaderRows.Length; i++)
		{
			foreach (int column in invaderColumns)
			{
				// Create instance of Invader
				Invader invader = InvaderScene.Instantiate<Invader>();
				// Set position of Invader
				invader.Position = getInvaderSpawnPosition(column, invaderRows[i]);
				// Add child
				AddChild(invader);
			}
			
		}
	}

	private void StartLevel()
	{
		SpawnInvaders();
	}


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		StartLevel();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
