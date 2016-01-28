using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DoorsOptimization  {

	/// <summary>
	/// Calculates the removable doors.
	/// </summary>
	/// <returns>The removable doors.</returns>
	/// <param name="cells">Cells.</param>
	public List<MazeDoor> CalculateRemovableDoors (ref MazeCell[,] cells) {
		var allDoors = ExtractDoors (cells);
		var toKeep = ExtractEssentialDoors (allDoors);
		return GetDublicatedDoors (allDoors, toKeep);
	}

	private List<MazeDoor> GetDublicatedDoors (List<MazeDoor> allDoors, List<MazeDoor> toKeep) {
		var doorsToRemove = new List<MazeDoor> ();
		foreach (MazeDoor door in allDoors) {  
			if (!toKeep.Contains (door))
				doorsToRemove.Add (door);
		}
		return doorsToRemove;
	}

	private List<MazeDoor> ExtractDoors (MazeCell[,] cells) {
		var allDoors = new List<MazeDoor>(); 
		foreach (var cell in cells) {
			
			var doors = GetDoorsContainedAtCell (cell);
			if (doors.Count () == 0 ) continue;
			foreach (var door in doors) {
				if (door.DoorDescription == Helpers.kDeletedDoorDescription) 
					continue;
				if (string.IsNullOrEmpty (door.DoorDescription))
					door.DoorDescription = CreateRoomName (door.cell.room.RoomId, door.otherCell.room.RoomId);
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

	private string CreateRoomName (int roomA, int roomB) {
		return (roomA > roomB) 
			? roomA + "-" + roomB 
			: roomB + "-" + roomA;
	}


	private List<MazeDoor> GetDoorsContainedAtCell(MazeCell cell) {
		var doors = new List<MazeDoor> ();
		foreach (var edge in cell.edges) {
			if (edge is MazeDoor)
				doors.Add ((MazeDoor)edge);
		}
		return doors;
	}

	/// <summary>
	/// Extracts the essential doors.
	/// Keeps a single door between two rooms.
	/// </summary>
	/// <returns>The essential doors.</returns>
	/// <param name="allDoors">All doors.</param>
	private List<MazeDoor> ExtractEssentialDoors (List<MazeDoor> allDoors){
		Dictionary<string, MazeDoor> doorsToKeepDictionary = new Dictionary<string, MazeDoor> ();
		foreach (var door in allDoors) {
			Helpers.AddToDictionary<string, MazeDoor> (doorsToKeepDictionary, door.DoorDescription, door);
		}
		return doorsToKeepDictionary.Values.ToList();
	}
		
}