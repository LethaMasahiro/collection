using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCharacterController : MonoBehaviour
{
    public new Transform camera;

    public float movementSpeed = 1;

    public float mouseSensitivity = 1;
    [Range(10, 90)]
    public float maxAngle = 80;

    [Range(-90, 10)]
    public float minAngle = -80;

    private Vector3 movement;

    private new Rigidbody rigidbody;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // if (Input.(KeyCode.Escape)) {
        //     Cursor.lockState = CursorLockMode.None;
        // }
        if (Input.GetMouseButtonDown(0)) {
            if (Cursor.lockState == CursorLockMode.Locked) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        if (Cursor.lockState != CursorLockMode.Locked) return;
        
        // camera
        float h = mouseSensitivity * Input.GetAxis("Mouse X");
        float v = mouseSensitivity * Input.GetAxis("Mouse Y");

        camera.Rotate(new Vector3(-v, 0, 0), Space.Self);
        transform.Rotate(new Vector3(0, h, 0), Space.Self);

        var rotEulerX = camera.rotation.eulerAngles.x;

        if (rotEulerX > -minAngle && rotEulerX < 360 - maxAngle) {
            if (v < 0) camera.Rotate(new Vector3(-minAngle - rotEulerX, 0, 0), Space.Self);
            else if (v > 0) camera.Rotate(new Vector3(360 - maxAngle - rotEulerX, 0, 0), Space.Self);
        }

        // movement
        float moveFB = Input.GetAxis("Vertical");
        float moveLR = Input.GetAxis("Horizontal");

        movement = moveFB * transform.forward + moveLR * transform.right;
        movement.Normalize();
    }

    private void FixedUpdate() {
        if (Cursor.lockState == CursorLockMode.Locked) {
            // camera
            float h = mouseSensitivity * Input.GetAxis("Mouse X");
            float v = mouseSensitivity * Input.GetAxis("Mouse Y");

            camera.Rotate(new Vector3(-v, 0, 0), Space.Self);
            transform.Rotate(new Vector3(0, h, 0), Space.Self);

            var rotEulerX = camera.rotation.eulerAngles.x;

            if (rotEulerX > -minAngle && rotEulerX < 360 - maxAngle) {
                if (v < 0) camera.Rotate(new Vector3(-minAngle - rotEulerX, 0, 0), Space.Self);
                else if (v > 0) camera.Rotate(new Vector3(360 - maxAngle - rotEulerX, 0, 0), Space.Self);
            }

            // movement
            float moveFB = Input.GetAxis("Vertical");
            float moveLR = Input.GetAxis("Horizontal");

            movement = moveFB * transform.forward + moveLR * transform.right;
            movement.Normalize();
        }
        
        rigidbody.velocity = movement * movementSpeed;
    }
}
