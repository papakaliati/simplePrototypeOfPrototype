using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class Helpers {

	public const string kDeletedDoorDescription = "deleted";

	/// <summary>
	/// Adds to dictionary.
	/// If Key already pressent, it overrides it.
	/// </summary>
	/// <param name="dict">Dictionary.</param>
	/// <param name="t1">T1, Dictionary key Type.</param>
	/// <param name="t2">T2 Dictionary value Type.</param>
	/// <typeparam name="T1">Dictionary key.</typeparam>
	/// <typeparam name="T2">Dictionary value.</typeparam>
	public static void AddToDictionary<T1,T2> ( IDictionary<T1,T2> dict, T1 t1, T2 t2) {
		if (dict.ContainsKey (t1))
			dict [t1] = t2;
		else
			dict.Add (t1, t2);
	}

	/// <summary>
	/// Extracts the type of the object of.
	/// If more than one public variable with that name, the first will always be returned.
	/// If none found, null will be returned so a null check on the caller is required.
	/// </summary>
	/// <returns>The object of type.</returns>
	/// <param name="parentClass">Parent class.</param>
	/// <typeparam name="T">The type of the variable requested.</typeparam>
	public static T ExtractObjectOfType<T>(object parentClass) {
		var fieldValues = parentClass.GetType()
			.GetFields()
			.Select(field => field.GetValue(parentClass))
			.ToList();
		return fieldValues.OfType<T>().Cast<T>().FirstOrDefault ();
	}

	public static bool CanDestroy (System.DateTime time) {
		return time <= System.DateTime.Now;
	}

}