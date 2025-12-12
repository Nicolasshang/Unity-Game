using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLock : MonoBehaviour {
    private InputAction pauseAction;
    private InputAction unpauseAction;

    void Awake() {
        pauseAction = InputSystem.actions.FindAction("Pause");
        unpauseAction = InputSystem.actions.FindAction("Unpause");
    }

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update() {
        if (pauseAction.WasPressedThisFrame()) {
            PauseGame();
        }

        if (unpauseAction.WasPressedThisFrame()) {
            UnpauseGame();
        }
    }

    public void PauseGame() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;

        Debug.Log("game was paused");
    }

    public void UnpauseGame() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;

        Debug.Log("game was unpaused");
    }
}
