using UnityEngine;

public class MazeDoor : MazePassage {

	private static Quaternion
		normalRotation = Quaternion.Euler(0f, -90f, 0f),
		mirroredRotation = Quaternion.Euler(0f, 90f, 0f);

	private int count;                          // The number of colliders present that should open the doors.

	public Transform hinge;

	private bool isMirrored;

	public bool isDoorOpen = false;

	private MazeDoor OtherSideOfDoor {
		get {
			return otherCell.GetEdge(direction.GetOpposite()) as MazeDoor;
		}
	}


	private GameObject player;                  // Reference to the player GameObject.

	void Awake() {
		//player = GameObject.FindWithTag ("Player");
	}

	void OnTriggerEnter (Collider other)
	{
		// If the triggering gameobject is the player...
		if(other.gameObject.tag == "Player")
		{
			// ... if this door requires a key...
		
				// If the door doesn't require a key, increase the count of triggering objects.
				count++;

		}
		// If the triggering gameobject is an enemy...

	}




	public override void Initialize (MazeCell primary, MazeCell other, MazeDirection direction) {
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

//	public override void OnPlayerEntered () {
//		OtherSideOfDoor.hinge.localRotation = hinge.localRotation = isMirrored ? mirroredRotation : normalRotation;
//		OtherSideOfDoor.cell.room.Show();
//	}
//	
//	public override void OnPlayerExited () {
//		OtherSideOfDoor.hinge.localRotation = hinge.localRotation = Quaternion.identity;
//		OtherSideOfDoor.cell.room.Hide();
//	}

	public override void OnPlayerEntered (bool canOpenDoor) {
		if (!canOpenDoor) return;

		if (isDoorOpen && canOpenDoor) {
			CloseDoor ();
			return;
		}
		if (!isDoorOpen)
			isDoorOpen = true;
		
		OtherSideOfDoor.isDoorOpen = true;

		OtherSideOfDoor.hinge.localRotation = hinge.localRotation = isMirrored ? mirroredRotation : normalRotation;
		OtherSideOfDoor.cell.room.Show();
	}

	private void CloseDoor() {
		OtherSideOfDoor.hinge.localRotation = hinge.localRotation = Quaternion.identity;
		OtherSideOfDoor.cell.room.Hide();
	//	OnPlayerExited (true);
		isDoorOpen = false;
		OtherSideOfDoor.isDoorOpen = false;
	}

	public override void OnPlayerExited (bool canOpenDoor) {
		return;

		if (!canOpenDoor && !isDoorOpen) return;
		OtherSideOfDoor.hinge.localRotation = hinge.localRotation = Quaternion.identity;
	    OtherSideOfDoor.cell.room.Hide();
	}
}