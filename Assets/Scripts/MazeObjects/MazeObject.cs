using UnityEngine;

public abstract class MazeObject : MonoBehaviour {

	public MazeCell cell;

	public MazeDirection direction;

	public virtual void Initialize (MazeCell cell, MazeDirection direction = MazeDirection.North) {
		this.cell = cell;
		this.direction = direction;
		transform.parent = cell.transform;
		transform.localPosition = Vector3.zero;
		transform.localRotation = direction.ToRotation();
	}
}
