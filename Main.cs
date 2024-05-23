using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Main : Node2D
{
	[Export]
	public PackedScene InvaderScene { get; set; }

	[Export]
	public PackedScene PathingScene { get; set; }

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
