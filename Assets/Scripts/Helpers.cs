using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Helpers {

	public const string kDeletedDoorDescription = "deleted";

	public static void AddToDictionary<T1,T2> ( IDictionary<T1,T2> dict, T1 t1, T2 t2) {
		if (dict.ContainsKey (t1))
			dict [t1] = t2;
		else
			dict.Add (t1, t2);
	}

}