using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class CapsuleMovement : MonoBehaviour {

    private InputAction jumpAction;
    private InputAction moveAction;
    private InputAction lookAction;
    public Rigidbody rigidBody;

    public float jumpAmount;   // magnitude of the jump vector
    public float moveSpeed;
    public float sprintSpeed;
    public float inputSensitivity;

    private bool isJumping;
    private bool isMoving;
    private bool isLooking;

    public Vector3 gravityVector;

    void Awake() {
        inputSensitivity = GameControl.Control.inputSensitivity;
        gravityVector = new Vector3(0f, -GameControl.Control.gravitationalConstant, 0f);

        jumpAction = InputSystem.actions.FindAction("Jump");
        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");

        rigidBody = GetComponent<Rigidbody>();
    }

    void Start() {
        rigidBody.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        rigidBody.linearDamping = 0f;
    }

    void Update() {
        if (jumpAction.WasPressedThisFrame() && IsGrounded()) isJumping = true;
        if (moveAction.IsPressed()) isMoving = true;
        Rotate();
    }

    void FixedUpdate() {
        rigidBody.AddForce(gravityVector, ForceMode.Acceleration);

        if (isJumping) {
            isJumping = false;
            Jump();
        }

        if (isMoving) {
            isMoving = false;
            Move();
        }
    }

    private bool IsGrounded() {
        float groundedDistance = 2.1f;  // May change later
        if (
            Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundedDistance)
            && rigidBody.linearVelocity.y == 0
        ) {
            return true;
        } else {
            return false;
        }
    }

    private void Jump() {
        rigidBody.AddForce(Vector3.up * jumpAmount, ForceMode.Impulse);
    }

    private void Move() {
        Vector2 directionVector = moveAction.ReadValue<Vector2>();

        // Angle between direction vector and the horizontal
        float angleWithHorizontal = Vector2.SignedAngle(Vector2.right, directionVector) * (Mathf.PI / 180f);

        if (angleWithHorizontal < 0) angleWithHorizontal += 2 * Mathf.PI;

        // The amount that the camera rotated by.
        // The 360f is here because we need delta y to increase if we rotate counterclockwise.
        float deltaYAngle = (360f - rigidBody.transform.eulerAngles.y) * (Mathf.PI / 180f);

        Vector3 transformedDirectionVector = new(Mathf.Cos(angleWithHorizontal + deltaYAngle), 0f, Mathf.Sin(angleWithHorizontal + deltaYAngle));

        // Debug.Log($"directionVector: [{directionVector.x}, {directionVector.y}], angleWithHorizontal: {angleWithHorizontal}, deltaYAngle: {deltaYAngle}, transformed vector: [{transformedDirectionVector.x}, 0, {transformedDirectionVector.z}]");

        rigidBody.MovePosition(rigidBody.transform.position + transformedDirectionVector * moveSpeed * Time.fixedDeltaTime);
    }

    private void Rotate() {
        float yRotation = GameControl.Control.playerYRotation;
        rigidBody.MoveRotation(Quaternion.Euler(0f, yRotation, 0f));
    }
}
