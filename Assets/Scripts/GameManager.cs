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

<<<<<<< HEAD
		new MazeOptimization (mazeInstance);
		new ObjectsPlacement (mazeInstance);
=======
		var optimization = new MazeOptimization (mazeInstance);
		var selections = new ObjectsPlacement (mazeInstance);
>>>>>>> 982e27a5b55b60cfd5ff5fa6de61483fbe69d5af
	}
}