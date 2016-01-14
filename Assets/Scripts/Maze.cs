﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Maze : MonoBehaviour {

	#region Public Properties

	public IntVector2 size;

	public MazeCell cellPrefab;

	public MazePassage passagePrefab;

	public MazeDoor doorPrefab;

	[Range(0f, 1f)]
	public float doorProbability;

	public MazeWall[] wallPrefabs;

	public MazeRoomSettings[] roomSettings;

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

	#region Public Methods

	public void Generate () {
		cells = new MazeCell[size.x, size.z];
		List<MazeCell> activeCells = new List<MazeCell>();
		DoFirstGenerationStep(activeCells);
		while (activeCells.Count > 0) 
			DoNextGenerationStep(activeCells);	
		//	for (int i = 0; i < rooms.Count; i++) 
		//	rooms[i].Hide();
	}

	#endregion

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
		else otherCell.Initialize (cell.room);
	}

	private void SameRoomAction (MazeCell cell, MazeCell otherCell){
		MazeRoom roomToAssimilate = otherCell.room;
		cell.room.Assimilate (roomToAssimilate);
		rooms.Remove (roomToAssimilate);
		Destroy (roomToAssimilate);
	}
		
	private void CreateWall (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		MazeWall wall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]) as MazeWall;
		wall.Initialize(cell, otherCell, direction);
		if (otherCell == null) return;
		wall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]) as MazeWall;
		wall.Initialize(otherCell, cell, direction.GetOpposite());
	}

	private MazeRoom CreateRoom (int indexToExclude) {
		MazeRoom newRoom = ScriptableObject.CreateInstance<MazeRoom>();
		newRoom.settingsIndex = Random.Range(0, roomSettings.Length);
		if (newRoom.settingsIndex == indexToExclude) 
			newRoom.settingsIndex = (newRoom.settingsIndex + 1) % roomSettings.Length;
		newRoom.settings = roomSettings[newRoom.settingsIndex];
		rooms.Add(newRoom);
		return newRoom;
	}

	#endregion
}