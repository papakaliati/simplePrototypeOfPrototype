using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class Helpers {

	public const string kDeletedDoorDescription = "deleted";

	public static void AddToDictionary<T1,T2> ( IDictionary<T1,T2> dict, T1 t1, T2 t2) {
		if (dict.ContainsKey (t1))
			dict [t1] = t2;
		else
			dict.Add (t1, t2);
	}

	public static T ExtractObjectOfType<T>(object parentClass) {
		var fieldValues = parentClass.GetType()
			.GetFields()
			.Select(field => field.GetValue(parentClass))
			.ToList();
		return fieldValues.OfType<T>().Cast<T>().FirstOrDefault ();
	}

}