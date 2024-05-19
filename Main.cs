using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Main : Node2D
{
	[Export]
	public PackedScene InvaderScene { get; set; }

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
