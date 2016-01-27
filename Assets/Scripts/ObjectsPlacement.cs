using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectsPlacement {

	private Maze maze;
	private MazeCell playerStartingCell;
	public static List<MazeObject> mazeObjects = new List<MazeObject>();
	private List<MazeDoor> usedDoors = new List<MazeDoor> ();

	public ObjectsPlacement (Maze maze) {
		this.maze = maze;
		SortRooms ();

		PlayerPlacement (0);
		GenerateDoorControllingObject<ControlPanel> (maze.controlPanel, 0);
		GenerateDoorControllingObject<Npc> (maze.npc, 2);
		GenerateInterractableObject<CargoBay> (maze.cargoBay, 1, SelectMiddleCell (1));
	}

	private void PlayerPlacement(int index) {
		var playerCharacter = GameObject.Find("Player");
		if (playerCharacter == null) return;
		playerStartingCell = SelectMiddleCell(index);
		playerCharacter.transform.position = playerStartingCell.transform.position;
	}

	private void GenerateInterractableObject<T>(T obj, int roomIndex, MazeCell cell)
		where T : InterractableMazeObject {
		InterractableObjectPlacement (cell, obj);
	}

	private void InterractableObjectPlacement<T>(MazeCell cell, T obj)
		where T : InterractableMazeObject {
		var item = Maze.Instantiate(obj) as T;
		item.Initialize (cell);
		mazeObjects.Add (item as MazeObject);
	}

	private void GenerateDoorControllingObject<T>(T obj, int roomIndex)
		where T : DoorControllingInterraclableMazeObject {
		var selectedRoomId = maze.rooms [roomIndex].RoomId;
		var door = maze.rooms [roomIndex].DoorsList
			.Where (x => (x.cell.room.RoomId == selectedRoomId ||
				x.otherCell.room.RoomId == selectedRoomId) && !usedDoors.Contains(x) ).FirstOrDefault ();
		var cells = MazeCell.GetSurroundingCellsInSameRoom (door.cell, door.otherCell, maze.rooms[roomIndex]);
		var cell = cells[Random.Range(0, cells.Count -1 )];
		DoorControllingObjectPlacement<T> (cell, door, obj);
	}

	private void DoorControllingObjectPlacement<T>(MazeCell cell, MazeDoor door, T obj)
		where T : DoorControllingInterraclableMazeObject {
		var item = Maze.Instantiate(obj) as T;
		item.Initialize (cell, door.direction, door);
		mazeObjects.Add (item as MazeObject);
		usedDoors.Add (door);
	}

	private void SortRooms () {
		maze.rooms.Sort((x, y) =>  x.DoorsList.Count.CompareTo(y.DoorsList.Count));
	}

	private MazeCell SelectMiddleCell (int index) {
		var dict = new Dictionary<int, MazeCell> ();
		foreach (var cell in maze.rooms [index].cells)
			Helpers.AddToDictionary<int, MazeCell> (dict, cell.Complexity, cell);
		return dict.OrderByDescending (i => i.Key).Select(x => x.Value).FirstOrDefault();
	}

}
