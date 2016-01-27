using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController))]

public class PlayerMotor : MonoBehaviour {

    [SerializeField]
    private Camera cam;
    private Vector3 Velocity;
    private Vector3 Rotation;
    private Vector3 CameraRotation;
    private Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    public void move(Vector3 _velocity) {
        Velocity = _velocity;
    }

    public void rotate(Vector3 _rotation) {
        Rotation = _rotation;
    }

    public void rotateCamera(Vector3 _CameraRotation) {
        CameraRotation = _CameraRotation;
    }

    void FixedUpdate() {
        PerformMovement();
        PerformRotation();
        PerformCameraRotation();
    }

    private void PerformMovement() {
        if (Velocity != Vector3.zero)
            rb.MovePosition(rb.position + Velocity * Time.fixedDeltaTime);
    }

    private void PerformRotation() {
        if (Rotation != Vector3.zero)
            rb.MoveRotation(rb.rotation * Quaternion.Euler(Rotation));
    }

	private void PerformCameraRotation () {
		if (CameraRotation != Vector3.zero && cam != null)
			cam.transform.Rotate (-CameraRotation);
	}
}
