using UnityEngine;

public class GameControl : MonoBehaviour {
    public static GameControl control;
    public static GameControl Control {
        get {
            if (control == null) {
                control = FindAnyObjectByType<GameControl>();
                if (control == null) {
                    GameObject gameObject = new("GameControl");
                    control = gameObject.AddComponent<GameControl>();
                }
            }
            return control;
        }
    }

    void Awake() {
        if (control == null) {
            DontDestroyOnLoad(gameObject);
        } else if (control != this) {
            control = this;
            Destroy(gameObject);
        }
    }

    void Update() {}

    public float inputSensitivity = 40f;
    public float gravitationalConstant = 9.81f;
    public float playerXRotation;
    public float playerYRotation;
    public float playerZRotation;
}
