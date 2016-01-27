using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Maze mazePrefab;

	private Maze mazeInstance;


	public void RestartGame () {
//		DestroyImmediate (mazeInstance.controlBox.gameObject);
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

		if (!mazeInstance.IsGeneratedMazeAccepted ()) {
			RestartGame ();
			return;
		}

		var optimization = new MazeOptimization (mazeInstance);
		var selections = new ObjectsPlacement (mazeInstance);
	}
}