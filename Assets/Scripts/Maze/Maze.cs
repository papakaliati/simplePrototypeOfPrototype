using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]

public class WallSettings {
	public MazeWall[] wallPrefabs;
	[Tooltip("Total sum should be 100.")]
	[Header("Walls Attribution Rate, sum needs to be 100, otherwise even distribution")]
	[Range(0, 100)]
	public int[] wallPropabilityAttributes ;
}

public class Maze : MonoBehaviour {

	#region Public Properties

	public IntVector2 size;

	public MazeCell cellPrefab;

	public MazePassage passagePrefab;

	public MazeDoor doorPrefab;

	[Range(0f, 1f)]
	public float doorProbability;

	public WallSettings wallSettings;

	public RoomDecoration[] roomDecorations;

	public MazeRoomSettings[] roomSettings;

	public MazeCell[,] cells;

	public List<MazeRoom> rooms = new List<MazeRoom>();

	public List<int> RoomSizes = new List<int>();

	#endregion

	public void Generate () {
		cells = new MazeCell[size.x, size.z];

		var mazeGeneration = new RandomDoorPropabilityMaze (this);
		mazeGeneration.GenerateMaze ();

	}

}