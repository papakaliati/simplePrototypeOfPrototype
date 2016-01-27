using UnityEngine;
using System.Collections;

public class InterractableMazeObject : MazeObject {

	public bool IsPlayerInProximity;

<<<<<<< HEAD
	public virtual void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag =="Player")
			IsPlayerInProximity = true;
	}

	public virtual void OnTriggerExit(Collider collider) {
=======
	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag =="Player")
			IsPlayerInProximity = true;
	}
	void OnTriggerExit(Collider collider) {
>>>>>>> 982e27a5b55b60cfd5ff5fa6de61483fbe69d5af
		if (collider.gameObject.tag =="Player")
			IsPlayerInProximity = false;
	}

}
