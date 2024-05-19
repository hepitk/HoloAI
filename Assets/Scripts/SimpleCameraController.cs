using UnityEngine;

public class SimpleCameraController : MonoBehaviour {
    public float movementSpeed = 5.0f;
    public float mouseSensitivity = 100.0f;

    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start() {
        // Lock the cursor to the center of the screen
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        // WASD movement
        float x = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        float z = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;

        transform.Translate(x, 0, z);

        // Mouse Look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, -9999999f, 9999999f);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        //transform.parent.Rotate(Vector3.up * mouseX);
    }
}
