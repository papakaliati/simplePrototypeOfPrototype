using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DoorsOptimization : MonoBehaviour {
	
	private static string _deleted = "deleted";

	public void OptimizeDoors (ref MazeCell[,] cells) {
		var allDoors = GetAllDoors (cells);
		var toKeep = GetDoorsToKeep (allDoors);
		SortDoors (allDoors, toKeep);
	}

	private void SortDoors (List<MazeDoor> allDoors, List<MazeDoor> toKeep) {
		foreach (MazeDoor door in allDoors)
			if (!toKeep.Contains (door)) {
				door.DoorDescription = _deleted;
				door.cell.room.DoorsList.Remove (door);
				Destroy (door.gameObject);
				Destroy (door);
			}
	}

	private List<MazeDoor> GetAllDoors (MazeCell[,] cells) {
		var allDoors = new List<MazeDoor>(); 
		foreach (var cell in cells) {
			var doors = GetDoorsContainedAtCell (cell);
			if (doors.Count () == 0 ) continue;
			foreach (var door in doors) {
				if (door.DoorDescription == _deleted) continue;
				if (string.IsNullOrEmpty(door.DoorDescription))
					door.DoorDescription = door.cell.room.RoomId + "-" + door.otherCell.room.RoomId;
				door.cell.room.DoorsList.Add (door);
				door.Rooms = new MazeRoom[] {
					door.cell.room,
					door.otherCell.room
				};
				allDoors.Add (door);
			}
		}
		return allDoors;
	}


	private List<MazeDoor> GetDoorsContainedAtCell(MazeCell cell) {
		var doors = new List<MazeDoor> ();
		foreach (var edge in cell.edges)
			if (edge is MazeDoor)
				doors.Add ((MazeDoor)edge);
		return doors;
	}

	private List<MazeDoor> GetDoorsToKeep (List<MazeDoor> allDoors){
		Dictionary<string, MazeDoor> doorsToKeepDictionary = new Dictionary<string, MazeDoor> ();
		foreach (var door in allDoors) {
			if (doorsToKeepDictionary.ContainsKey (door.DoorDescription))
				doorsToKeepDictionary [door.DoorDescription] = door;
			else
				doorsToKeepDictionary.Add (door.DoorDescription, door);
		}
		return doorsToKeepDictionary.Values.ToList();
	}



}

