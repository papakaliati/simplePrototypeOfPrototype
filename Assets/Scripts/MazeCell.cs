using UnityEngine;

public class MazeCell : MonoBehaviour {

	public IntVector2 coordinates;

	public MazeRoom room;

	private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];

	private int initializedEdgeCount;

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

	public void Initialize (MazeRoom room) {
		room.Add(this);
		transform.GetChild(0).GetComponent<Renderer>().material = room.settings.floorMaterial;
	}

	public MazeCellEdge GetEdge (MazeDirection direction) {
		return edges[(int)direction];
	}

	public void SetEdge (MazeDirection direction, MazeCellEdge edge) {
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