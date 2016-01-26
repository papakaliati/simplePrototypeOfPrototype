﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Maze mazePrefab;

	private Maze mazeInstance;


	public void RestartGame () {
		Destroy (mazeInstance.gameObject);
		Destroy (mazeInstance);
		Debug.ClearDeveloperConsole ();
		BeginGame();
	}

	private void Start () {
	   BeginGame();
	}
	
	private void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			RestartGame(); 
		}
	}

	private void BeginGame () {
		mazeInstance = Instantiate(mazePrefab) as Maze;
		mazeInstance.Generate();

//		if (!mazeInstance.AcceptedNumberOfRoomsInMaze())
//			RestartGame ();
	}
}