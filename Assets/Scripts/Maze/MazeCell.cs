using UnityEngine;
using System.Collections.Generic;

public class MazeCell : MonoBehaviour {

	public IntVector2 coordinates;

	public MazeRoom room;

	public MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];

	private int initializedEdgeCount;

	public int WallCount { private set; get;}

	public int Complexity;

	public bool IsFullyInitialized {
		get {
			return initializedEdgeCount == MazeDirections.Count;
		}
	}
		
	public MazeDirection RandomUninitializedDirection() {
		int skips = Random.Range (0, MazeDirections.Count - initializedEdgeCount);
		for (int i = 0; i < MazeDirections.Count; i++) {
			if (edges [i] != null) continue;
			if (skips == 0) return (MazeDirection)i;
			-- skips;
		}
		throw new System.InvalidOperationException ("Something is Off");
	}

	public static List<MazeCell> GetAllNeighborhoodCellsInSameRoom(MazeCell cell, int factor) {
		List<MazeCell> cells = new List<MazeCell>();
		foreach (IntVector2 vectors in MazeDirections.vectorsAllDirections) {
			IntVector2 tempVector = new IntVector2(0,0);
			for (int k = 0; k < factor+1; ++k)
				tempVector += vectors ;
			IntVector2 coordinates = cell.coordinates + tempVector;
			var celly = cell.room.cells.Find (x => x.coordinates == coordinates);
			if (celly != null)
				cells.Add (celly);
		}
		return cells;
	}

	public static List<MazeCell> GetSurroundingCellsInSameRoom(MazeCell cell, MazeCell otherCell, MazeRoom room) {
		var selectedCell = cell.room.RoomId == room.RoomId ? cell : otherCell;

		List<MazeCell> cells = new List<MazeCell>();
		foreach (IntVector2 vectors in MazeDirections.vectorsAllDirections) {
			IntVector2 coordinates = selectedCell.coordinates + vectors;
			var celly = selectedCell.room.cells.Find (x => x.coordinates == coordinates);
			if (celly != null)
				cells.Add (celly);
		}
		return cells;
	}
			
	public void Initialize (MazeRoom room) {
		room.Add(this);
		transform.GetChild(0).GetComponent<Renderer>().material = room.settings.floorMaterial;
	}

	public MazeCellEdge GetEdge (MazeDirection direction) {
		return edges[(int)direction];
	}

	public void SetEdge (MazeDirection direction, MazeCellEdge edge) {
		if (!(edge is MazePassage))
			++WallCount;
		edges[(int)direction] = edge;
		++ initializedEdgeCount;
	}

	public void Show () {
		gameObject.SetActive(true);
	}

	public void Hide () {
		gameObject.SetActive(false);
	}
}