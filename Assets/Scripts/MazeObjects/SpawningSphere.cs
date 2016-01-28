using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpawningSphere : MazeObject {

	private System.DateTime killTime;

	public void Update() {
		if (!Helpers.CanDestroy(killTime)) return;
		Destroy (this.gameObject);
		Destroy (this);
	}

	public void Initialize (MazeCell cell) {
		base.Initialize (cell);
		Vector3 temp = new Vector3(0,6f,0);
		this.transform.position += temp;
		var killTimer = Random.Range (3, 5);
		this.killTime = System.DateTime.Now.AddSeconds (killTimer);
	}
}