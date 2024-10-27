using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraTransform;
    public float normalSpeed;
    public float fastSpeed;
    public float movementSpeed;
    public float movementTime;
    public float rotationAmount;
    public float maxZoom;
    public float minZoom;
    [SerializeField] public Vector3 zoomAmount;

    [SerializeField] public Vector3 newPosition;
    [SerializeField] public Quaternion newRotation;
    [SerializeField] public Vector3 newZoom;

    [SerializeField] public Vector3 dragStartPosition;
    [SerializeField] public Vector3 dragCurrentPosition;

    [SerializeField] private Quaternion startRotation;

    public float edgeScrollSpeed = 10f;
    public float edgeThickness = 10f;

    public Vector3 minPosition;
    public Vector3 maxPosition;

    void Start()
    {
        newPosition = transform.position;
        startRotation = transform.rotation;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;

        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        HandleMouseInput();
        HandleMovementInput();
    }

    void HandleMouseInput()
    {
        // Zoom mit dem Mausrad
        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
        }

        if (Input.GetMouseButtonDown(2))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }

        // Halten der mittleren Maustaste für Draggen
        if (Input.GetMouseButton(2))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);
                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
        }
    }

    void HandleMovementInput()
    {
        // Kamera-Geschwindigkeit erhöhen, wenn Shift gedrückt wird
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = fastSpeed;
        }
        else
        {
            movementSpeed = normalSpeed;
        }

        // Kamera durch Tastatureingabe bewegen
        if (Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (transform.forward * movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (transform.forward * -movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (transform.right * -movementSpeed * Time.deltaTime);
        }

        // Kamera durch Bildschirmrand-Scrolling bewegen
        if (Input.mousePosition.x <= edgeThickness)
        {
            newPosition += transform.right * -edgeScrollSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - edgeThickness)
        {
            newPosition += transform.right * edgeScrollSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= edgeThickness)
        {
            newPosition += transform.forward * -edgeScrollSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y >= Screen.height - edgeThickness)
        {
            newPosition += transform.forward * edgeScrollSpeed * Time.deltaTime;
        }

        // Kamera-Rotation mit Tasten
        if (Input.GetKey(KeyCode.N) || Input.GetKey(KeyCode.PageUp))
        {
        float rotationY = Mathf.Clamp(newRotation.eulerAngles.y + rotationAmount, startRotation.eulerAngles.y - 45f, startRotation.eulerAngles.y + 45f);
        newRotation = Quaternion.Euler(newRotation.eulerAngles.x, rotationY, newRotation.eulerAngles.z);
        }
        else if (Input.GetKey(KeyCode.M) || Input.GetKey(KeyCode.PageDown))
        {
        float rotationY = Mathf.Clamp(newRotation.eulerAngles.y - rotationAmount, startRotation.eulerAngles.y - 45f, startRotation.eulerAngles.y + 45f);
        newRotation = Quaternion.Euler(newRotation.eulerAngles.x, rotationY, newRotation.eulerAngles.z);
        }
        else
        {
        newRotation = Quaternion.Lerp(newRotation, startRotation, Time.deltaTime * movementTime);
        }

        //zoom mit tasten
        if (Input.GetKey(KeyCode.Home))
        {
            newZoom += zoomAmount;
        }
        if (Input.GetKey(KeyCode.End))
        {
            newZoom -= zoomAmount;
        }


        
        newPosition.x = Mathf.Clamp(newPosition.x, minPosition.x, maxPosition.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minPosition.y, maxPosition.y);
        newPosition.z = Mathf.Clamp(newPosition.z, minPosition.z, maxPosition.z);

        
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);

        
        newZoom.y = Mathf.Clamp(newZoom.y, minZoom, maxZoom);
        newZoom.z = Mathf.Clamp(newZoom.z, -maxZoom, -minZoom);
        Vector3 targetPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
        cameraTransform.localPosition = targetPosition;
    }
}
