using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectsPlacement {

	private Maze maze;
	private MazeCell playerStartingCell;
	private List<MazeDoor> usedDoors = new List<MazeDoor> ();

	public ObjectsPlacement (Maze maze) {
		this.maze = maze;
		SortRooms ();
		PlayerPlacement (maze.rooms[0]);

		GenerateObjects ();
	}

	/// <summary>
	/// Sorts the rooms according to door count ascending.
	/// </summary>
	private void SortRooms () {
		maze.rooms.Sort((x, y) =>  x.DoorsList.Count.CompareTo(y.DoorsList.Count));
	}
		
	private void PlayerPlacement(MazeRoom room) {
		var playerCharacter = GameObject.Find("Player");
		if (playerCharacter == null) return;
		playerStartingCell = SelectMiddleCell(room);
		playerCharacter.transform.position = playerStartingCell.transform.position;
	}

	private void GenerateObjects() {
		var dict = new Dictionary<InterractableMazeObject, MazeRoom> () {
			{maze.controlPanel, maze.rooms [0]},
			{maze.cargoBay, maze.rooms [1]},
			{maze.npc, maze.rooms [2]},
		};
		AddControlsToRooms (dict);
	}

	private System.Action<object,MazeRoom> GetFunc (InterractableMazeObject key) {
		if (key is ControlPanel)
			return GenerateDoorControllingObject<ControlPanel>;
		else if (key is Npc)
			return GenerateDoorControllingObject<Npc>;
		else if (key is CargoBay)
			return GenerateInterractableObject<CargoBay>;
		else
			return null;
	}

	private void AddControlsToRooms (Dictionary <InterractableMazeObject, MazeRoom> dict) {
		foreach (var pair in dict) {
			var action = GetFunc (pair.Key);
			if (action == null) continue;
			action (maze, pair.Value);
		}
	}

	private void GenerationMethod () {

	}

	private void GenerateInterractableObject<T>(object parentClass, MazeRoom room)
			where T : InterractableMazeObject {
		var obj = Helpers.ExtractObjectOfType<T> (parentClass);
		if (obj == null) return;
		var cell = SelectMiddleCell (room);
		InterractableObjectPlacement (cell, obj);
	}

	private void InterractableObjectPlacement<T>(MazeCell cell, T obj)
			where T : InterractableMazeObject {
		var item = Maze.Instantiate(obj) as T;
		item.Initialize (cell);
		maze.mazeObjects.Add (item as MazeObject);
	}

	/// <summary>
	/// Generates the door controlling object.
	/// Current Implementation always puts it close to the controlling door.
	/// </summary>
	/// <param name="parentClass">Parent class.</param>
	/// <param name="roomIndex">Room index.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	private void GenerateDoorControllingObject<T>(object parentClass, MazeRoom room)
			where T : DoorControllingInterraclableMazeObject {
		var obj = Helpers.ExtractObjectOfType<T> (parentClass);
		if (obj == null) return;

		var selectedRoomId = room.RoomId;
		var door = room.DoorsList
			.Where (x => ( x.cell.room.RoomId == selectedRoomId || x.otherCell.room.RoomId == selectedRoomId)
				&& !usedDoors.Contains(x) )
			.FirstOrDefault ();
		
		var cells = MazeCell.GetSurroundingCellsInSameRoom (door.cell, door.otherCell, room);
		var cell = cells[Random.Range(0, cells.Count -1 )];
		DoorControllingObjectPlacement<T> (cell, door, obj);
	}

	private void DoorControllingObjectPlacement<T>(MazeCell cell, MazeDoor door, T obj)
		where T : DoorControllingInterraclableMazeObject {
		var item = Maze.Instantiate(obj) as T;
		item.Initialize (cell, door.direction, door);
		maze.mazeObjects.Add (item as MazeObject);
		usedDoors.Add (door);
	}

	private MazeCell SelectMiddleCell (MazeRoom room) {
		var dict = new Dictionary<int, MazeCell> ();
		foreach (var cell in room.cells)
			Helpers.AddToDictionary<int, MazeCell> (dict, cell.Complexity, cell);
		return dict.OrderByDescending (i => i.Key).Select(x => x.Value).FirstOrDefault();
	}

}
