using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectsPlacement {

	private Maze maze;
	public static List<MazeObject> mazeObjects = new List<MazeObject>();

	public ObjectsPlacement (Maze maze) {
		this.maze = maze;
		SortRooms ();
		FirstRoomPlacement ();
	}

	private void FirstRoomPlacement() {
		var playerCharacter = GameObject.Find("Player");
		var startingCell = SelectStartingCell();

		if (playerCharacter != null)
			playerCharacter.transform.position = startingCell.transform.position;

		var ControlBox = Maze.Instantiate(maze.controlBox) as ControlBox;
		var door = maze.rooms[0].DoorsList[0];

		var cells = MazeCell.GetSurroundingCellsInSameRoom (door.cell, door.otherCell, startingCell.room);
		var controlBoxCell = cells[Random.Range(0, cells.Count -1 )];
		ControlBox.Initialize (controlBoxCell, door.direction, door);

 		mazeObjects.Add (ControlBox as MazeObject);
	}

	private void SortRooms () {
		maze.rooms.Sort((x, y) => x.Size.CompareTo(y.Size));
	}
		
	private MazeCell SelectStartingCell () {
		var dict = new Dictionary<int, MazeCell> ();
		foreach (var cell in maze.rooms [0].cells)
			Helpers.AddToDictionary<int, MazeCell> (dict, cell.Complexity, cell);
		return dict.OrderByDescending (i => i.Key).Select(x => x.Value).FirstOrDefault();
	}

}
