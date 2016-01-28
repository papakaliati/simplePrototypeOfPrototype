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
	public float doorProbability = 0.05f;

	[Header("It overrides the doorProbability if greater than zero")]
	public int MaxDoorNumber = 0;

	public WallSettings wallSettings;

	public MazeObject[] roomDecorations;

	public MazeRoomSettings[] roomSettings;

	public MazeCell[,] cells;

	public SpawningSphere spawningSphere;

	public Npc npc;

	public ControlPanel controlPanel;

	public CargoBay cargoBay;

	[HideInInspector]
	public List<MazeRoom> rooms = new List<MazeRoom>();

	[HideInInspector]
	public List<int> RoomSizes = new List<int>();

	[HideInInspector]
	public RandomDoorPropabilityMaze randomDoorPropabilityMaze;

	[HideInInspector]
	public List<MazeObject> mazeObjects = new List<MazeObject>();


	#endregion

	public static Maze maze;

	public Maze() {
		maze = this;
	}

	public void Generate () {
		cells = new MazeCell[size.x, size.z];
		randomDoorPropabilityMaze = new RandomDoorPropabilityMaze (this);
		randomDoorPropabilityMaze.GenerateMaze ();
	}

	public bool IsGeneratedMazeAccepted () {
	//	return true;
		return rooms.Count == 3 && (rooms.Count( x => x.Size > 5 )) == 3;
	}

}