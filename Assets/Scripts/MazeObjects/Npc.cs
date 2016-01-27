using UnityEngine;
using System.Collections;

public class Npc : DoorControllingInterraclableMazeObject {

	private string[] NPCTalk = new string[7];
	private string[] PCTalk = new string[7];
	private int myIndex = 0;
	public bool myTalking = false;

	public override void OnTriggerExit(Collider collider) {
		if (collider.gameObject.tag == "Player") {
			myTalking = false;
			IsPlayerInProximity = false;
		}
	}

	private void Start() {
		NPCTalk [0] = "Clck to open door!";
		NPCTalk [1] = "Clck to close door!";
		PCTalk [0] = "Open Sesame!";
		PCTalk [1] = "Close Sesame!";
		myTalking = false;
	}

	public void OnGUI () {
		if (myTalking && IsPlayerInProximity) {
			GUI.Label (new Rect (40, 200, 350, 120), NPCTalk [myIndex]);
			if (GUI.Button (new Rect (20, 270, 350, 30), PCTalk [myIndex])) {
				myIndex = myIndex >= 1 ? 0 : ++myIndex;
				controlledDoor.DoorInterraction ();
			}
		}
	}
}