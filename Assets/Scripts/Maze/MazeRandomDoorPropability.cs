using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RandomDoorPropabilityMaze {
	public Maze maze;

	public RandomDoorPropabilityMaze (Maze maze) {
		this.maze = maze;
	}

	public void GenerateMaze () {
		List<MazeCell> activeCells = new List<MazeCell> ();
		DoFirstGenerationStep (activeCells);
		while (activeCells.Count > 0)
			DoNextGenerationStep (activeCells);
	}

	private MazeCell GetCell (IntVector2 coordinates) {
		return maze.cells[coordinates.x, coordinates.z];
	}

	private IntVector2 RandomCoordinates {
		get {
			return new IntVector2(Random.Range(0, maze.size.x), Random.Range(0, maze.size.z));
		}
	}

	private bool ContainsCoordinates (IntVector2 coordinate) {
		return coordinate.x >= 0 && coordinate.x < maze.size.x && coordinate.z >= 0 && coordinate.z < maze.size.z;
	}

	private MazeCell CreateCell (IntVector2 coordinates) {
		MazeCell newCell = Maze.Instantiate(maze.cellPrefab) as MazeCell;
		maze.cells[coordinates.x, coordinates.z] = newCell;
		newCell.coordinates = coordinates;
		newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
		newCell.transform.parent = maze.transform;
		newCell.transform.localPosition = new Vector3(coordinates.x - maze.size.x * 0.5f + 0.5f, 0f, coordinates.z - maze.size.z * 0.5f + 0.5f);
		return newCell;
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
			CreateWall (currentCell, null, direction); //Outer Wall
			return;
		}

		MazeCell neighbor = GetCell(coordinates);
		if (neighbor == null) {
			neighbor = CreateCell(coordinates);
			GeneratePassageDifferentRoom(currentCell, neighbor, direction);
			activeCells.Add(neighbor);
		}
		else if (currentCell.room.settingsIndex == neighbor.room.settingsIndex) 
			GeneratePassageSameRoom(currentCell, neighbor, direction);
		else 
			CreateWall(currentCell, neighbor, direction);
	}

	int doorNumber = 0;
	private MazePassage GetMazePassageBasedOnMaxDoorNumber (MazeCell cell) {
		if (cell.room != null && cell.room.Size > maze.cells.Length/(maze.MaxDoorNumber+2)  && doorNumber < maze.MaxDoorNumber) {
			doorNumber++;
			return maze.doorPrefab;
		}
		else
			return maze.passagePrefab;
	}

	private MazePassage GetMazePassageBasedOnDoorPropability() {
		return Random.value < maze.doorProbability ? maze.doorPrefab : maze.passagePrefab;
	}

	private void GeneratePassageDifferentRoom (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		MazePassage prefab;
		if (maze.MaxDoorNumber == 0)
			prefab = GetMazePassageBasedOnDoorPropability ();
		else 
			prefab = GetMazePassageBasedOnMaxDoorNumber (cell);
		MazePassage passage = Maze.Instantiate(prefab) as MazePassage;
		passage.Initialize(cell, otherCell, direction);
		passage = Maze.Instantiate(prefab) as MazePassage;
		DifferentRoomAction (cell, otherCell, passage);
		passage.Initialize (otherCell, cell, direction.GetOpposite ());
	}

	private void GeneratePassageSameRoom (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		var prefab = maze.passagePrefab;
		MazePassage passage = Maze.Instantiate(prefab) as MazePassage;
		passage.Initialize(cell, otherCell, direction);
		passage = Maze.Instantiate(prefab) as MazePassage;
		passage.Initialize (otherCell, cell, direction.GetOpposite ());
		if (cell.room != otherCell.room) 
			RoomAssimilation (cell, otherCell);
	}

	private void DifferentRoomAction (MazeCell cell, MazeCell otherCell, MazePassage passage) {
		if (passage is MazeDoor)
			otherCell.Initialize (CreateRoom (cell.room.settingsIndex));
		else 
			otherCell.Initialize (cell.room);
	}

	public void RoomAssimilation (MazeCell cell, MazeCell otherCell) {
		MazeRoom roomToAssimilate = otherCell.room;
		cell.room.Assimilate (roomToAssimilate);
		maze.rooms.Remove (roomToAssimilate);
		Maze.Destroy (roomToAssimilate);
	}

	public void WallGeneration (MazeCell cell, MazeCell otherCell, MazeDirection direction, int[] randomNumbers) {
		MazeWall wall = Maze.Instantiate (maze.wallSettings.wallPrefabs [randomNumbers[0]]) as MazeWall;
		wall.Initialize (cell, otherCell, direction);
		if (otherCell == null) return;
		wall = Maze.Instantiate (maze.wallSettings.wallPrefabs [randomNumbers[1]]) as MazeWall;
		wall.Initialize (otherCell, cell, direction.GetOpposite ());
	}

	private int[] GetRandomNumberForWallPrefab () {
		int randomNumber1, randomNumber2;
		if (maze.wallSettings.wallPropabilityAttributes.Count () != maze.wallSettings.wallPrefabs.Count () 
			|| maze.wallSettings.wallPropabilityAttributes.Sum () != 100) {
			randomNumber1 = Random.Range (0, maze.wallSettings.wallPrefabs.Length);
			randomNumber2 = Random.Range (0, maze.wallSettings.wallPrefabs.Length);
		}
		else {
			randomNumber1 = PropabiliesCalulations<int>.GetRandomSelection (maze.wallSettings.wallPropabilityAttributes);
			randomNumber2 = PropabiliesCalulations<int>.GetRandomSelection (maze.wallSettings.wallPropabilityAttributes);
		}
		return new int[] {randomNumber1, randomNumber2};
	}

	public void CreateWall (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		var randomNumbers = GetRandomNumberForWallPrefab ();
		WallGeneration (cell, otherCell, direction, randomNumbers);
	}

	private int roomCounter = 0;

	private MazeRoom CreateRoom (int indexToExclude) {
		MazeRoom newRoom = ScriptableObject.CreateInstance<MazeRoom>();
		newRoom.RoomId = roomCounter++;
		newRoom.settingsIndex = Random.Range(0, maze.roomSettings.Length);
		if (newRoom.settingsIndex == indexToExclude) 
			newRoom.settingsIndex = (newRoom.settingsIndex + 1) % maze.roomSettings.Length;
		newRoom.settings = maze.roomSettings[newRoom.settingsIndex];
		maze.rooms.Add(newRoom);
		return newRoom;
	}

}