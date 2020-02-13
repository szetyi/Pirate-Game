using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private Vector3 dragOrigin;
    float dragSpeed = 30;
    public float minSize = 20;
    public float maxSize = 50;
    public float zoomSensitivity = 20f;
    [Range(0.01f,0.1f)]
    public float wasdModifier = 0.05f;
    [Range(0.01f, 0.1f)]
    public float edgeScrollModifier = 0.05f;
    [Range(1f, 3f)]
    public float dragSpeedModifier = 1.5f;

    private Vector3 pos;

    // Update is called once per frame
    void Update() {
        CamZoom();
        DragCam();
        EdgeScrolling();
        WasdCamera();
    }

    // Zooms using the Scroll Wheel, and adjusts speed accordingly.
    private void CamZoom() {
        float fov = Camera.main.orthographicSize;

        fov -= Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
        fov = Mathf.Clamp(fov, minSize, maxSize);
        dragSpeed = fov;
        dragSpeed = Mathf.Clamp(dragSpeed, minSize, maxSize-10);
        Camera.main.orthographicSize = fov;

    }


    // Enables panning around the map using the mouse when the middle mouse button is held down.
    private void DragCam() {

        if (Input.GetMouseButtonDown(2)) {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(2)) return;
        else {
            pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(-pos.x * dragSpeed * dragSpeedModifier, -pos.y * dragSpeed * dragSpeedModifier, 0);
            dragOrigin = Input.mousePosition;

            transform.Translate(move, Space.World);
        }
    }

    // Moves the camera when the mouse is near the edges.
    private void EdgeScrolling() {
        pos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector3 move = new Vector3(0, 0, 0);

        if (pos.x < 0.01f && pos.y < 0.01f) {
            move.x = dragSpeed * -edgeScrollModifier;
            move.y = dragSpeed * -edgeScrollModifier;
            transform.Translate(move);
        }else if (pos.x > 0.99f && pos.y < 0.01f) {
            move.x = dragSpeed * edgeScrollModifier;
            move.y = dragSpeed * -edgeScrollModifier;
            transform.Translate(move);
        }else if (pos.x < 0.01f && pos.y > 0.99f) {
            move.x = dragSpeed * -edgeScrollModifier;
            move.y = dragSpeed * edgeScrollModifier;
            transform.Translate(move);
        }else if (pos.x > 0.99f && pos.y > 0.99f) {
            move.x = dragSpeed * edgeScrollModifier;
            move.y = dragSpeed * edgeScrollModifier;
            transform.Translate(move);
        }else if (pos.x < 0.01f) {
            move.x = dragSpeed * -edgeScrollModifier;
            transform.Translate(move); 
        }else if (pos.x > 0.99f) {
            move.x = dragSpeed * edgeScrollModifier;
            transform.Translate(move);
        }else if (pos.y < 0.01f) {
            move.y = dragSpeed * -edgeScrollModifier;
            transform.Translate(move);
        }else if (pos.y > 0.99f) {
            move.y = dragSpeed * edgeScrollModifier;
            transform.Translate(move);
        }
    }

    // Moves the camera using the W, A, S, D keys.
    private void WasdCamera() {
        Vector3 move = new Vector3(0, 0, 0);

        if (Input.GetKey("a") && Input.GetKey("s")) {
            move.x = dragSpeed * -wasdModifier;
            move.y = dragSpeed * -wasdModifier;
            transform.Translate(move, Space.Self);
        }
        else if (Input.GetKey("d") && Input.GetKey("s")) {
            move.x = dragSpeed * wasdModifier;
            move.y = dragSpeed * -wasdModifier;
            transform.Translate(move, Space.Self);
        }
        else if (Input.GetKey("a") && Input.GetKey("w")) {
            move.x = dragSpeed * -wasdModifier;
            move.y = dragSpeed * wasdModifier;
            transform.Translate(move, Space.Self);
        }
        else if (Input.GetKey("d") && Input.GetKey("w")) {
            move.x = dragSpeed * wasdModifier;
            move.y = dragSpeed * wasdModifier;
            transform.Translate(move, Space.Self);
        }
        else if (Input.GetKey("a")) {
            move.x = dragSpeed * -wasdModifier;
            transform.Translate(move, Space.Self);
        }
        else if (Input.GetKey("d")) {
            move.x = dragSpeed * wasdModifier;
            transform.Translate(move, Space.Self);
        }
        else if (Input.GetKey("s")) {
            move.y = dragSpeed * -wasdModifier;
            transform.Translate(move, Space.Self);
        }
        else if (Input.GetKey("w")) {
            move.y = dragSpeed * wasdModifier;
            transform.Translate(move, Space.Self);
        }
    }
}
