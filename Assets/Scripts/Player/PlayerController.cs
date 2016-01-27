﻿using UnityEngine;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(PlayerMotor))]

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float ScreenSpeed = 5f;
    [SerializeField]
    private float RotationSpeed = 3f;

    private PlayerMotor motor;

    void Start()
    {
        motor = GetComponent<PlayerMotor>();
    }

<<<<<<< HEAD
	private void Interractions () {
		CargoBayInterraction ();
		DoorInterraction ();
		NpcInterraction ();
	}

	private void CargoBayInterraction() {
		var obj = ObjectsPlacement.mazeObjects
			.OfType<CargoBay> ()
			.Where (x => x.IsPlayerInProximity)
			.Select (x => x as CargoBay).FirstOrDefault ();
		if (obj == null) return;

		obj.SpawnSphereObject ();
	}

	private void NpcInterraction() {
		var obj = ObjectsPlacement.mazeObjects
			.OfType<Npc> ()
			.Where (x => x.IsPlayerInProximity)
			.Select (x => x as Npc).FirstOrDefault ();
		if (obj == null) return;

		obj.myTalking = !obj.myTalking;
	}

	private void DoorInterraction ()	{
		var obj = ObjectsPlacement.mazeObjects.OfType<ControlPanel> ()
			.Where (x => x.IsPlayerInProximity)
			.Select (x => x as ControlPanel).FirstOrDefault ();
		if (obj == null) return;
		var door = obj.controlledDoor;

		door.DoorInterraction ();
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.R)) 
			Interractions ();
		float xMovement = Input.GetAxisRaw ("Horizontal");
		float zMovement = Input.GetAxisRaw ("Vertical");
		Vector3 MoveHorizontal = transform.right * xMovement;
		Vector3 MoveVertical = transform.forward * zMovement;
		Vector3 Velocity = (MoveHorizontal + MoveVertical).normalized * ScreenSpeed;
		motor.move (Velocity);
		float yRot = Input.GetAxisRaw ("Mouse X");
		Vector3 rotation = new Vector3 (0, yRot, 0) * RotationSpeed;
		motor.rotate (rotation);
		CameraUpdate ();
	}

    void CameraUpdate() {
=======
	private void DoorInterraction ()	{
		DoorControllingInterraclableMazeObject obj = ObjectsPlacement.mazeObjects.OfType<DoorControllingInterraclableMazeObject> ()
			.Where (x => x.IsPlayerInProximity)
			.Select (x => x as DoorControllingInterraclableMazeObject).FirstOrDefault ();
		if (obj == null) return;
		var door = obj.controlledDoor;
		if (door is MazeDoor)
			((MazeDoor)door).DoorInterraction ();
	}
		
    void Update() {
		if (Input.GetKeyDown (KeyCode.R))
			DoorInterraction ();

		float xMovement = Input.GetAxisRaw ("Horizontal");
		float zMovement = Input.GetAxisRaw ("Vertical");

		Vector3 MoveHorizontal = transform.right * xMovement;
		Vector3 MoveVertical = transform.forward * zMovement;

		//Final Movement Vector
		Vector3 Velocity = (MoveHorizontal + MoveVertical).normalized * ScreenSpeed;

		//Apply motion
		motor.move (Velocity);

		//Get rotation

		float yRot = Input.GetAxisRaw ("Mouse X");

		Vector3 rotation = new Vector3 (0, yRot, 0) * RotationSpeed;

		//Apply rotation
		motor.rotate (rotation);
		CameraUpdate ();

        
	}

    void CameraUpdate()
    {
        //Get camera rotation

>>>>>>> 982e27a5b55b60cfd5ff5fa6de61483fbe69d5af
        float xRot = Input.GetAxisRaw("Mouse Y");
        Vector3 CameraRotation = new Vector3(xRot, 0, 0) * RotationSpeed;
        motor.rotateCamera(CameraRotation);
    }
}
