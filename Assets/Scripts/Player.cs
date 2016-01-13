using UnityEngine;

public class Player : MonoBehaviour {

	private MazeCell currentCell;

	private MazeDirection currentDirection;

	public void SetLocation (MazeCell cell) {
		if (currentCell != null) {
			currentCell.OnPlayerExited(canOpenDoor);
		}
		currentCell = cell;
		transform.localPosition = cell.transform.localPosition;
		currentCell.OnPlayerEntered(canOpenDoor);
	}

	private void Move (MazeDirection direction) {
		MazeCellEdge edge = currentCell.GetEdge(direction);
		if (edge is MazeDoor) {
			var door = edge as MazeDoor;
			if (canOpenDoor || door.isDoorOpen)
				SetLocation(edge.otherCell);
			return;
		}
		if (edge is MazePassage) {
			SetLocation(edge.otherCell);
		}
	}

	private void Look (MazeDirection direction) {
		transform.localRotation = direction.ToRotation();
		currentDirection = direction;
	}

	private bool canOpenDoor;

	public void OpenDoor(MazeDirection direction) {
		canOpenDoor = true;
		currentCell.OnPlayerEntered(canOpenDoor);

	}

	private void Update () {
		if (canOpenDoor)
			canOpenDoor = false;

		if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
			Move(currentDirection);
		}
		else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
			Move(currentDirection.GetNextClockwise());
		}
		else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
			Move(currentDirection.GetOpposite());
		}
		else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
			Move(currentDirection.GetNextCounterclockwise());
		}
		else if (Input.GetKeyDown(KeyCode.Q)) {
			Look(currentDirection.GetNextCounterclockwise());
		}
		else if (Input.GetKeyDown(KeyCode.E)) {
			Look(currentDirection.GetNextClockwise());
		}
		else if (Input.GetKeyDown(KeyCode.R)) 
			OpenDoor(currentDirection);
	}
}