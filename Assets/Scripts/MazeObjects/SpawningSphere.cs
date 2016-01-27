using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpawningSphere : MazeObject {

	public static Dictionary<SpawningSphere, System.DateTime> killDictionary;

	public void Update() {
		var list = killDictionary.Where (x => x.Value <= System.DateTime.Now).Select (y => y.Key).ToList();
		foreach (var item in list) {
			killDictionary.Remove (item);
			Destroy (item.gameObject);
			Destroy (item);
		}
	}

	public void Initialize (MazeCell cell) {
		base.Initialize (cell);
		Vector3 temp = new Vector3(0,6f,0);
		this.transform.position += temp;
		if (killDictionary == null) killDictionary = new Dictionary<SpawningSphere, System.DateTime> ();
		var killTimer = Random.Range (3, 5);
		killDictionary.Add (this, System.DateTime.Now.AddSeconds(killTimer));
	}

//	public update

}