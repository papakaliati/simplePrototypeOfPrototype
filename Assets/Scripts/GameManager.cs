using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Maze mazePrefab;

	private Maze mazeInstance;


	public void RestartGame () {
		BeginGame();
	}

	private void Start () {
	   BeginGame();
	}
	
	private void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			; //	RestartGame(); 
		}
	}

	private void BeginGame () {
//		Camera.main.clearFlags = CameraClearFlags.Skybox;
//		Camera.main.rect = new Rect(0f, 0f, 1f, 1f);

		mazeInstance = Instantiate(mazePrefab) as Maze;
		mazeInstance.Generate();

//		Camera.main.clearFlags = CameraClearFlags.Depth;
//		Camera.main.rect = new Rect(0f, 0f, 0.5f, 0.5f);

		for (var i = 0; i < 3; i++)
			if (mazeInstance.mazeComplexity.AcceptablePlacementLocations.Count == 0)
				RestartGame ();
	}
}