using UnityEngine;
using System.Linq;

public class CargoBay : InterractableMazeObject {

	public void SpawnSphereObject() {
		var item = Maze.Instantiate(Maze.maze.spawningSphere) as SpawningSphere;
		var random = Random.Range( 0, this.cell.room.cells.Count()-1);
		item.Initialize (this.cell.room.cells[random]);
	}
}