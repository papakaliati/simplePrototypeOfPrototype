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

	public MazeComplexity mazeComplexity { private set ; get;}
		 
	#endregion

	#region private Properties

	private MazeCell[,] cells;

	private List<MazeRoom> rooms = new List<MazeRoom>();

	private IntVector2 RandomCoordinates {
		get {
			return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
		}
	}

	#endregion

	public void Generate () {
		cells = new MazeCell[size.x, size.z];
		List<MazeCell> activeCells = new List<MazeCell>();
		DoFirstGenerationStep(activeCells);
		while (activeCells.Count > 0) 
			DoNextGenerationStep(activeCells);	
		mazeComplexity = new MazeComplexity(rooms);

		var doorsOptimization = new DoorsOptimization ();
		var doorsToBeRemoved = doorsOptimization.CalculateRemovableDoors (ref cells);
		SortDoors (doorsToBeRemoved);
		CreateRoomsToDoors ();

		// For Testing Only
		PrintRoomsAndDoors ();
	}

	private void SortDoors (List<MazeDoor> doorsToRemove) {
		foreach (MazeDoor door in doorsToRemove) {
			door.DoorDescription = Helpers.kDeletedDoorDescription;

			var edge = door.otherCell.GetEdge ( MazeDirections.GetOpposite(door.direction));
			if (edge is MazeDoor) {
				if (((MazeDoor)edge).DoorDescription == Helpers.kDeletedDoorDescription)
					CreateWall (door.cell, door.otherCell, door.direction);
			}

			door.cell.room.DoorsList.Remove (door);
			Destroy (door.gameObject);


			Destroy (door.gameObject);
			Destroy (door);
		}
	}
		
	private void PrintRoomsAndDoors() { 
		var text = new System.Text.StringBuilder ();
		foreach (var room in rooms) {			
			text.AppendLine (string.Format(" Room : {0}, size : {1}, Door Number : {2}", room.RoomId, room.Size, room.DoorsList.Count ()));
			foreach (var door in room.DoorsList) 
				text.AppendLine (string.Format(" Door Name : {0}, cell : {1}",door.DoorDescription, door.cell.name));
		}
		Debug.Log (text);
	}

	private void CreateRoomsToDoors () {
		Dictionary<MazeRoom, List<MazeDoor>> RoomsToDoors = new Dictionary<MazeRoom, List<MazeDoor>>();
		var doors = new List<MazeDoor> ();
		foreach (var room in rooms) {
			RoomsToDoors.Add (room, new List<MazeDoor> ());
			foreach (var door in room.DoorsList)
				doors.Add (door);
		}

		//Reset Doors list in order to be recreated
		foreach (var room in rooms)
			room.DoorsList = new List<MazeDoor> ();
				
		foreach (var door in doors) {
			var roomIds = GetRoomsFromDoorDescription (door.DoorDescription);
			var foundRooms = RoomsToDoors.Keys.Where (x => roomIds.Contains (x.RoomId)).ToList ();
			foreach (var element in foundRooms) {
				element.DoorsList.Add (door);
			//	RoomsToDoors [element].Add (door);
			}
		}
	}
		

	private int[] GetRoomsFromDoorDescription(string doorDescription) {
		char[] delimiters = new char[] { '-'};
		var ids =  doorDescription.Split (delimiters).Select (x => System.Convert.ToInt32 (x)).ToArray ();
		return ids;
	}
		
	#region Private Methods
		
	private enum RoomType {
		DifferentRoom,
		SameRoom
	}

	private bool ContainsCoordinates (IntVector2 coordinate) {
		return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
	}
		
	private MazeCell GetCell (IntVector2 coordinates) {
		return cells[coordinates.x, coordinates.z];
	}

	private void DoFirstGenerationStep (List<MazeCell> activeCells) {
		MazeCell newCell = CreateCell(RandomCoordinates);
		newCell.Initialize(CreateRoom(-1));
		activeCells.Add(newCell);
	}

	private void DoNextGenerationStep (List<MazeCell> activeCells) {
		int currentIndex = activeCells.Count - 1;
		MazeCell currentCell = activeCells[currentIndex];

		if (currentCell.IsFullyInitialized) {
			activeCells.RemoveAt(currentIndex);
			return;
		}

		MazeDirection direction = currentCell.RandomUninitializedDirection();
		IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();

		if (!ContainsCoordinates (coordinates)) {
			CreateWall (currentCell, null, direction);
			return;
		}
			
		MazeCell neighbor = GetCell(coordinates);
		if (neighbor == null) {
			neighbor = CreateCell(coordinates);
			GeneratePassage(currentCell, neighbor, direction, RoomType.DifferentRoom);
			activeCells.Add(neighbor);
		}
		else if (currentCell.room.settingsIndex == neighbor.room.settingsIndex) 
			GeneratePassage(currentCell, neighbor, direction, RoomType.SameRoom);
		else 
			CreateWall(currentCell, neighbor, direction);
	}

	private MazeCell CreateCell (IntVector2 coordinates) {
		MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
		cells[coordinates.x, coordinates.z] = newCell;
		newCell.coordinates = coordinates;
		newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
		newCell.transform.parent = transform;
		newCell.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);
		return newCell;
	}
		
	private void GeneratePassage (MazeCell cell, MazeCell otherCell, MazeDirection direction, RoomType roomType) {
		MazePassage prefab = roomType == RoomType.DifferentRoom
				? (Random.value < doorProbability ? doorPrefab : passagePrefab)
				: passagePrefab;
		
		MazePassage passage = Instantiate(prefab) as MazePassage;
		passage.Initialize(cell, otherCell, direction);
		passage = Instantiate(prefab) as MazePassage;

		if (roomType == RoomType.DifferentRoom) 
			DifferentRoomAction (cell, otherCell, passage);
		passage.Initialize (otherCell, cell, direction.GetOpposite ());
		if (roomType == RoomType.SameRoom && cell.room != otherCell.room) 
			SameRoomAction (cell, otherCell);
	}

	private void DifferentRoomAction (MazeCell cell, MazeCell otherCell, MazePassage passage) {
		if (passage is MazeDoor)
			 otherCell.Initialize (CreateRoom (cell.room.settingsIndex));
		else 
			otherCell.Initialize (cell.room);
	}

	private void SameRoomAction (MazeCell cell, MazeCell otherCell){
		MazeRoom roomToAssimilate = otherCell.room;
		cell.room.Assimilate (roomToAssimilate);
		rooms.Remove (roomToAssimilate);
		Destroy (roomToAssimilate);
	}

	public void WallGeneration (MazeCell cell, MazeCell otherCell, MazeDirection direction, int[] randomNumbers) {
		MazeWall wall = Instantiate (wallSettings.wallPrefabs [randomNumbers[0]]) as MazeWall;
		wall.Initialize (cell, otherCell, direction);
		if (otherCell == null) return;
		wall = Instantiate (wallSettings.wallPrefabs [randomNumbers[1]]) as MazeWall;
		wall.Initialize (otherCell, cell, direction.GetOpposite ());
	}

	private int[] GetRandomNumberForWallPrefab () {
		int randomNumber1, randomNumber2;
		if (wallSettings.wallPropabilityAttributes.Count () != wallSettings.wallPrefabs.Count () 
			|| wallSettings.wallPropabilityAttributes.Sum () != 100) {
			randomNumber1 = Random.Range (0, wallSettings.wallPrefabs.Length);
			randomNumber2 = Random.Range (0, wallSettings.wallPrefabs.Length);
		}
		else {
			randomNumber1 = PropabiliesCalulations<int>.GetRandomSelection (wallSettings.wallPropabilityAttributes);
			randomNumber2 = PropabiliesCalulations<int>.GetRandomSelection (wallSettings.wallPropabilityAttributes);
		}
		return new int[] {randomNumber1, randomNumber2};
	}

	private void CreateWall (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		var randomNumbers = GetRandomNumberForWallPrefab ();
		WallGeneration (cell, otherCell, direction, randomNumbers);
	}

	private int roomCounter = 0;
	private MazeRoom CreateRoom (int indexToExclude) {
		MazeRoom newRoom = ScriptableObject.CreateInstance<MazeRoom>();
		newRoom.RoomId = roomCounter++;
		newRoom.settingsIndex = Random.Range(0, roomSettings.Length);
		if (newRoom.settingsIndex == indexToExclude) 
			newRoom.settingsIndex = (newRoom.settingsIndex + 1) % roomSettings.Length;
		newRoom.settings = roomSettings[newRoom.settingsIndex];
		rooms.Add(newRoom);
		return newRoom;
	}

	#endregion
}