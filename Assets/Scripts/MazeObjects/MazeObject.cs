using UnityEngine;

public abstract class MazeObject : MonoBehaviour {

	public MazeCell cell;

	public MazeDirection direction;

<<<<<<< HEAD
	public virtual void Initialize (MazeCell cell, MazeDirection direction = MazeDirection.North) {
=======
	public virtual void Initialize (MazeCell cell, MazeDirection direction) {
>>>>>>> 982e27a5b55b60cfd5ff5fa6de61483fbe69d5af
		this.cell = cell;
		this.direction = direction;
		transform.parent = cell.transform;
		transform.localPosition = Vector3.zero;
		transform.localRotation = direction.ToRotation();
	}
}
