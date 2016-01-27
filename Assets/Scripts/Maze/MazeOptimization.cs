using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MazeOptimization {

	private MazeComplexity mazeComplexity { set ; get;}
	private Maze maze;

	public MazeOptimization (Maze maze) {
		this.maze = maze;
		mazeComplexity = new MazeComplexity (maze.rooms);
		var doorsOptimization = new DoorsOptimization ();
		var doorsToBeRemoved = doorsOptimization.CalculateRemovableDoors (ref maze.cells);
		RemoveExtraDoors (doorsToBeRemoved);
		CreateDoorList ();
		PrintRoomsAndDoors ();	// For Testing Only
	}

	/// <summary>
	/// Removes the extra doors and closes the gap wall on its place
	/// </summary>
	/// <param name="doorsToRemove">Doors to remove.</param>
	private void RemoveExtraDoors (List<MazeDoor> doorsToRemove) {
		foreach (MazeDoor door in doorsToRemove) {
			door.DoorDescription = Helpers.kDeletedDoorDescription;

			var edge = door.otherCell.GetEdge ( MazeDirections.GetOpposite(door.direction));
			if (edge is MazeDoor) {
				if (((MazeDoor)edge).DoorDescription == Helpers.kDeletedDoorDescription)
					maze.randomDoorPropabilityMaze.CreateWall (door.cell, door.otherCell, door.direction);
			}

			door.cell.room.DoorsList.Remove (door);
			Maze.Destroy (door.gameObject);
			Maze.Destroy (door);
		}
	}

	private void PrintRoomsAndDoors() { 
		var text = new System.Text.StringBuilder ();
		foreach (var room in maze.rooms) {			
			text.AppendLine (string.Format(" Room : {0}, size : {1}, Door Number : {2}", room.RoomId, room.Size, room.DoorsList.Count ()));
			foreach (var door in room.DoorsList) 
				text.AppendLine (string.Format(" Door Name : {0}, cell : {1}, complexity : {2}",
					door.DoorDescription, door.cell.name, door.cell.Complexity));
		}
		Debug.Log (text);
	}

	/// <summary>
	/// Creates the door list for each room
	/// </summary>
	private void CreateDoorList () {
		Dictionary<MazeRoom, List<MazeDoor>> RoomsToDoors = new Dictionary<MazeRoom, List<MazeDoor>>();
		var doors = new List<MazeDoor> ();
		foreach (var room in maze.rooms) {
			Helpers.AddToDictionary<MazeRoom, List<MazeDoor>>(RoomsToDoors, room, new List<MazeDoor> ());
			foreach (var door in room.DoorsList)
				doors.Add (door);
		}

		//Reset Doors list in order to be recreated
		foreach (var room in maze.rooms)
			room.DoorsList = new List<MazeDoor> ();

		foreach (var door in doors) {
			var roomIds = GetRoomsFromDoorDescription (door.DoorDescription);
			var foundRooms = RoomsToDoors.Keys.Where (x => roomIds.Contains (x.RoomId)).ToList ();
			foreach (var element in foundRooms) {
				element.DoorsList.Add (door);
			}
		}
	}

	private int[] GetRoomsFromDoorDescription(string doorDescription) {
		char[] delimiters = new char[] {'-'};
		var ids =  doorDescription.Split (delimiters).Select (x => System.Convert.ToInt32 (x)).ToArray ();
		return ids;
	}

}
