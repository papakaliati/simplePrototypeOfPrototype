using UnityEngine;
using System.Collections.Generic;

public class MazeDoor : MazePassage {

	private static Quaternion
		normalRotation = Quaternion.Euler(0f, -90f, 0f),
		mirroredRotation = Quaternion.Euler(0f, 90f, 0f);

	private int count;                     

	public Transform hinge;

	public string DoorDescription;

	public MazeRoom[] Rooms;

	private bool isMirrored;

	private bool isDoorOpen = false;

	private MazeDoor OtherSideOfDoor {
		get {
			return otherCell.GetEdge(direction.GetOpposite()) as MazeDoor;
		}
	}
		
	public void DoorInterraction () {
		if (isDoorOpen) {
			CloseDoor ();
			return;
		}
		isDoorOpen = true;
		hinge.localRotation = normalRotation;
	//	hinge.localRotation = hinge.localRotation = isMirrored ? mirroredRotation : normalRotation;
	}

	private void CloseDoor() {
		hinge.localRotation = hinge.localRotation = Quaternion.identity;
		isDoorOpen = false;
	}

	public override void Initialize (MazeCell primary, MazeCell other, MazeDirection direction) {
		isDoorOpen = false;
		base.Initialize(primary, other, direction);
		if (OtherSideOfDoor != null) {
			isMirrored = true;
			hinge.localScale = new Vector3(-1f, 1f, 1f);
			Vector3 p = hinge.localPosition;
			p.x = -p.x;
			hinge.localPosition = p;
		}
		for (int i = 0; i < transform.childCount; i++) {
			Transform child = transform.GetChild(i);
			if (child != hinge) {
				child.GetComponent<Renderer>().material = cell.room.settings.wallMaterial;
			}
		}
	}

}