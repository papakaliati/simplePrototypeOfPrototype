using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorsOptimization {

	private Dictionary <MazeRoom, Dictionary< MazeCell, int>> roomMapping = 
		new Dictionary<MazeRoom, Dictionary<MazeCell, int>>();

	public DoorsOptimization(Dictionary <MazeRoom, Dictionary< MazeCell, int>> roomMapping) {
		this.roomMapping = roomMapping;
	}

	private void CheckDoorsPerRoom() {
		foreach (var item in roomMapping) {
			var room = item.Key;
			var pair = item.Value;
			foreach (var element in pair) 
				if (element.Value == 0) {
					var cell = element.Key;
					var door = CheckIfCellContainsDoors (cell);
					if (door == null) continue;
					room.DoorsList.Add (door);
				}
		}
		PrintDoors ();
		OptimizeDoors ();
	}

	private void OptimizeDoors() {
		List<string> toRemove = new List<string>();

		foreach (var item in doorMultiplication)
			if (item.Value > 1)
				toRemove.Add (item.Key);
	}

	private Dictionary<MazeCell, string> doorConnections = new Dictionary<MazeCell, string>();
	private Dictionary<string, int> doorMultiplication = new Dictionary<string, int>();

	private void PrintDoors() {
		foreach (var item in roomMapping) {
			var room = item.Key;
			var pair = item.Value;
			Debug.LogFormat ( " Size of Room : {0} Number of doors  : {1}", room.RoomSize, room.DoorsList.Count );
			Debug.LogFormat (" Cell of Door : {0}", room.DoorsList [0].cell);
			foreach (var door in room.DoorsList) {
				Debug.LogFormat ( " Door : {0} Connecting Rooms : {1} - {2}", 
					door.cell.name , door.cell.room.RoomId, door.otherCell.room.RoomId );

				var connection = door.cell.room.RoomId.ToString() + door.otherCell.room.RoomId.ToString();

				doorConnections.Add (door.cell, connection);

				if (doorMultiplication.ContainsKey (connection))
					++doorMultiplication [connection];
				else
					doorMultiplication.Add (connection, 0);
			}
		}
	}

	private MazeDoor CheckIfCellContainsDoors(MazeCell cell) {
		foreach (var edge in cell.edges)
			if (edge is MazeDoor)
				return (MazeDoor)edge;
		return null;
	}

}
