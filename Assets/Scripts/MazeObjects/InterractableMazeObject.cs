using UnityEngine;
using System.Collections;

public class InterractableMazeObject : MazeObject {

	public bool IsPlayerInProximity;

	public virtual void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag =="Player")
			IsPlayerInProximity = true;
	}

	public virtual void OnTriggerExit(Collider collider) {
		if (collider.gameObject.tag =="Player")
			IsPlayerInProximity = false;
	}

}