using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public Transform target;

    public Vector3 offset;
    public float zoomSpeed = 4;
    public Vector2 minAndMaxZoom = new Vector2(5, 15);

    public float pitch = 2;

    public float yawSpeed = 100;

    private float currentZoom = 10;
    private float currentYaw = 0;


    void Update()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minAndMaxZoom.x, minAndMaxZoom.y);

        currentYaw -= Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime;
    }
    // LateUpdate is called once per end of frame
    void LateUpdate()
    {
        transform.position = target.position - offset * currentZoom;
        transform.LookAt(target.position + Vector3.up * pitch);

        transform.RotateAround(target.position, Vector3.up, currentYaw);
    }
}
