using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaySelections : MonoBehaviour {

	private List<MazeRoom> rooms;

	public PlaySelections(List<MazeRoom> rooms){
		this.rooms = rooms;
		SortRooms ();
		SelectStartingRoom ();

	}

	private void SortRooms () {
		rooms.Sort((x, y) => x.Size.CompareTo(y.Size));
		rooms.Reverse ();
	}

	private void SelectStartingRoom () {
		var sb = new System.Text.StringBuilder ();
		var k = rooms [0];
		foreach (var cell in k.cells)
			sb.AppendLine(string.Format("cell {0} complexity {1}", cell.name, cell.Complexity));
		Debug.Log (sb);
	}
}
