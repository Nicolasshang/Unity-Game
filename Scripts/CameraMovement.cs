using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour {
    
    public Rigidbody rigidBody;
    public GameObject currentMainCamera;
    private InputAction lookAction;

    public float inputSensitivity;

    void Awake() {
        inputSensitivity = GameControl.Control.inputSensitivity;
        lookAction = InputSystem.actions.FindAction("Look");
    }

    void Start() {
        currentMainCamera.transform.eulerAngles = new Vector3(0f, 0f, 0f);
    }

    void Update() {
        Vector2 lookVector = lookAction.ReadValue<Vector2>();

        MoveCameraToObject(currentMainCamera, rigidBody);
        RotateCamera(currentMainCamera, lookVector, inputSensitivity);
    }

    private void MoveCameraToObject(GameObject camera, Rigidbody rigidBody) {
        Vector3 rigidBodyPosition = rigidBody.transform.position;
        camera.transform.position = rigidBodyPosition;
    }

    private void RotateCamera(GameObject camera, Vector2 lookVector, float inputSensitivity) {
        Vector3 currentEulerAngles = camera.transform.eulerAngles;

        float xRotation = currentEulerAngles.x - lookVector.y * inputSensitivity * Time.deltaTime;
        float yRotation = currentEulerAngles.y + lookVector.x * inputSensitivity * Time.deltaTime;
        
        GameControl.Control.playerXRotation = xRotation;
        GameControl.Control.playerYRotation = yRotation;

        // xRotation = 90 when you look straight down, decreases to 0, wraps around to 360, and then decreases to 270 again when you look straight up (don't ask me why).
        if (xRotation <= 90 || xRotation >= 270) {
            camera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
    }
}
