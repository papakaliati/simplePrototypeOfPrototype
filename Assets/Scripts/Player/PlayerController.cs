using UnityEngine;
using System.Collections;

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

	float maxDistance = 10f;

    void Update() {
		try {
		RaycastHit Hit ;
		if (Input.GetKeyDown (KeyCode.R)
			&& Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out Hit, maxDistance)
			&& Hit.collider.tag == "Door") {
			var door = Hit.collider.GetComponent <MazeDoor> ();
			if (door is MazeDoor)
				((MazeDoor)door).DoorInterraction ();
			}
		} catch ( System.Exception ex) {
			Debug.Log (ex.Message);
			Debug.Log (ex);

		}

        float xMovement = Input.GetAxisRaw("Horizontal");
        float zMovement = Input.GetAxisRaw("Vertical");

        Vector3 MoveHorizontal = transform.right * xMovement;
        Vector3 MoveVertical = transform.forward * zMovement;

        //Final Movement Vector
        Vector3 Velocity = (MoveHorizontal + MoveVertical).normalized * ScreenSpeed;

        //Apply motion
        motor.move(Velocity);

        //Get rotation

        float yRot = Input.GetAxisRaw("Mouse X");

        Vector3 rotation = new Vector3(0, yRot, 0) * RotationSpeed;

        //Apply rotation
        motor.rotate(rotation);
        CameraUpdate();

        
    }

    void CameraUpdate()
    {
        //Get camera rotation

        float xRot = Input.GetAxisRaw("Mouse Y");

        Vector3 CameraRotation = new Vector3(xRot, 0, 0) * RotationSpeed;

        //Apply rotation
        motor.rotateCamera(CameraRotation);
    }
}
