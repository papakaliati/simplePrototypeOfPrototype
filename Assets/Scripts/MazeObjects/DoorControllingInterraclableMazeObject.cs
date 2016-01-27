using UnityEngine;
using System.Collections;

public class DoorControllingInterraclableMazeObject : InterractableMazeObject {

	public MazeDoor controlledDoor;

	public void Initialize (MazeCell cell, MazeDirection direction, MazeDoor door) {
		base.Initialize (cell, direction);
		this.controlledDoor = door;
	}

}
