using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MazeComplexity  {
	
	public Dictionary <MazeRoom, Dictionary< MazeCell, int>> AcceptableLocationsDictionary = 
		new Dictionary<MazeRoom, Dictionary<MazeCell, int>>();
	public Dictionary <MazeRoom, Dictionary< MazeCell, int>> roomMapping = 
		new Dictionary<MazeRoom, Dictionary<MazeCell, int>>();

	public MazeComplexity(List<MazeRoom> maze ) {
		CalculateMazeComplexity (maze);
	}
		
	//A room is considered available if it has size more than 10 cells
	private List<MazeRoom> GetAvailableRoomsForObjectPlacement (List<MazeRoom> rooms) {
		//return rooms.OrderByDescending (x => x.RoomSize).Where (x => x.RoomSize > 10).ToList ();
		return rooms;
	}

	private void CalculateMazeComplexity(List<MazeRoom> rooms){ 
		var availableRooms = GetAvailableRoomsForObjectPlacement (rooms);
		foreach (var room in availableRooms) 
			CalculateRoomComplexity (room);
		FilterDictionaries (); 
	}

	private void FilterDictionaries () {
		foreach (var pair in roomMapping) {
			Debug.LogFormat (" RoomID : {0} RoomSize  : {1}", pair.Key.RoomId, pair.Key.RoomSize); 
			foreach (var pairy in pair.Value)
				if (pairy.Value > 0) {
					Debug.LogFormat ("The Cell : {0} has complexity : {1}", pairy.Key.name, pairy.Value); 
					SaveComplexityToDictionary (pairy.Key, pair.Key, pairy.Value, AcceptableLocationsDictionary); 
				}
		}
	}

	private void CalculateRoomComplexity(MazeRoom room) {
		foreach (var cell in room.cells)
			CalculateCellComplexity (cell, room);
	} 

	private void CalculateCellComplexity(MazeCell cell, MazeRoom room) {
		var complexity = GetCellComplexity (cell, room);
		SaveComplexityToDictionary (cell, room, complexity, roomMapping);
	}

	private void SaveComplexityToDictionary (MazeCell cell, MazeRoom room, int complexity, Dictionary <MazeRoom, Dictionary< MazeCell, int>> dictionary){
		if (!dictionary.ContainsKey (room)) {
			var tempDick = new Dictionary<MazeCell,int> () { { cell, 0 } };
			dictionary.Add (room, tempDick);
		} else if (!dictionary [room].ContainsKey (cell))
			dictionary [room].Add (cell, complexity);
		else
			dictionary [room] [cell] = complexity;
	}

	private int GetCellComplexity(MazeCell cell, MazeRoom room) {
		int complexity = 0;
		bool edgeReached = false;

		while (!edgeReached) {
			var neighborhoodCells = MazeCell.GetNeighborhoodCells (cell, room, complexity);
			if (neighborhoodCells.Count < 8)
				edgeReached = true;
			else
				complexity++;
		}
		return complexity;
	}
}
