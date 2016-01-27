using UnityEngine;
using System.Collections;

public class InterractableMazeObject : MazeObject {

	public bool IsPlayerInProximity;

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag =="Player")
			IsPlayerInProximity = true;
	}
	void OnTriggerExit(Collider collider) {
		if (collider.gameObject.tag =="Player")
			IsPlayerInProximity = false;
	}

}
