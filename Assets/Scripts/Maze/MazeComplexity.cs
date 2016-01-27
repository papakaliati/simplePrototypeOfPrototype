using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MazeComplexity  {


	public MazeComplexity(List<MazeRoom> maze ) {
		CalculateMazeComplexity (maze);
	}
		
	private List<MazeRoom> GetAvailableRoomsForObjectPlacement (List<MazeRoom> rooms) {
		//return rooms.OrderByDescending (x => x.RoomSize).Where (x => x.RoomSize > 10).ToList ();
		return rooms;
	}

	private void CalculateMazeComplexity(List<MazeRoom> rooms){ 
		var availableRooms = GetAvailableRoomsForObjectPlacement (rooms);
		foreach (var room in availableRooms) 
			CalculateRoomComplexity (room);
	}

	private void CalculateRoomComplexity (MazeRoom room) {
		foreach (var cell in room.cells)
			CalculateCellComplexity (cell);
	}

	private void CalculateCellComplexity (MazeCell cell) {
		var complexity = GetCellComplexity (cell);
		cell.Complexity = complexity;
	}

	private int GetCellComplexity(MazeCell cell) {
		int complexity = 0;
		int counter = 0;
		bool edgeReached = false;

		while (!edgeReached) {
			var neighborhoodCells = MazeCell.GetAllNeighborhoodCellsInSameRoom (cell, counter++);
			complexity += neighborhoodCells.Count;
			if (neighborhoodCells.Count < 8) 
				edgeReached = true;	
		}
		return complexity;
	}
}
