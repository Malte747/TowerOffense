using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    UIManager uiManager;
    GameManager gameManager;

    public Transform cameraTransform;
    public float normalSpeed;
    public float fastSpeed;
    public float movementSpeed;
    public float movementTime;
    public float rotationAmount;
    public float maxZoom;
    public float minZoom;
    public float movementDuration;
    private bool isMoving = false;
    [SerializeField] public Vector3 zoomAmount;

    [SerializeField] public Vector3 newPosition;
    [SerializeField] public Quaternion newRotation;
    [SerializeField] public Vector3 newZoom;

    [SerializeField] public Vector3 dragStartPosition;
    [SerializeField] public Vector3 dragCurrentPosition;

    [SerializeField] private Quaternion startRotation;

    [SerializeField] public Vector3 targetPositionDefender;
    [SerializeField] public Vector3 targetPositionAttacker;

    public float edgeScrollSpeed = 10f;
    public float edgeThickness = 10f;

    public Vector3 minPosition;
    public Vector3 maxPosition;

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        newPosition = transform.position;
        startRotation = transform.rotation;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;

        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        if (gameManager.gameInProgress == false||isMoving || (uiManager != null && uiManager.isPaused)) return;

        HandleMouseInput();
        HandleMovementInput();
    }

    void HandleMouseInput()
    {
        if (uiManager != null && uiManager.isPaused) return;

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
        if (uiManager != null && uiManager.isPaused) return;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = fastSpeed;
        }
        else
        {
            movementSpeed = normalSpeed;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (transform.forward * movementSpeed * Time.unscaledDeltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (transform.forward * -movementSpeed * Time.unscaledDeltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * movementSpeed * Time.unscaledDeltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (transform.right * -movementSpeed * Time.unscaledDeltaTime);
        }

        if (Input.mousePosition.x <= edgeThickness)
        {
            newPosition += transform.right * -edgeScrollSpeed * Time.unscaledDeltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - edgeThickness)
        {
            newPosition += transform.right * edgeScrollSpeed * Time.unscaledDeltaTime;
        }
        if (Input.mousePosition.y <= edgeThickness)
        {
            newPosition += transform.forward * -edgeScrollSpeed * Time.unscaledDeltaTime;
        }
        if (Input.mousePosition.y >= Screen.height - edgeThickness)
        {
            newPosition += transform.forward * edgeScrollSpeed * Time.unscaledDeltaTime;
        }

        if (Input.GetKey(KeyCode.N) || Input.GetKey(KeyCode.PageUp))
        {
            float rotationY = Mathf.Clamp(newRotation.eulerAngles.y + rotationAmount, startRotation.eulerAngles.y + 45f, startRotation.eulerAngles.y - 45f);
            newRotation = Quaternion.Euler(newRotation.eulerAngles.x, rotationY, newRotation.eulerAngles.z);
        }
        else if (Input.GetKey(KeyCode.M) || Input.GetKey(KeyCode.PageDown))
        {
            float rotationY = Mathf.Clamp(newRotation.eulerAngles.y - rotationAmount, startRotation.eulerAngles.y - 45f, startRotation.eulerAngles.y + 45f);
            newRotation = Quaternion.Euler(newRotation.eulerAngles.x, rotationY, newRotation.eulerAngles.z);
        }
        else
        {
            newRotation = Quaternion.Lerp(newRotation, startRotation, Time.unscaledDeltaTime * movementTime);
        }

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

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.unscaledDeltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.unscaledDeltaTime * movementTime);

        newZoom.y = Mathf.Clamp(newZoom.y, minZoom, maxZoom);
        newZoom.z = Mathf.Clamp(newZoom.z, -maxZoom, -minZoom);
        Vector3 targetPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.unscaledDeltaTime * movementTime);
        cameraTransform.localPosition = targetPosition;
    }



    public void MoveCameraToDefenderPosition()
    {
        StartCoroutine(MoveCameraCoroutine(targetPositionDefender, movementDuration));
    }

    public void MoveCameraToAttackerPosition()
    {
        StartCoroutine(MoveCameraCoroutine(targetPositionAttacker, movementDuration));
    }


    private IEnumerator MoveCameraCoroutine(Vector3 targetPosition, float duration)
    {
    isMoving = true;
    Vector3 initialPosition = transform.position;
    float elapsedTime = 0f;

    while (elapsedTime < duration)
    {
        float progress = Mathf.SmoothStep(0, 1, elapsedTime / duration);
        transform.position = Vector3.Lerp(initialPosition, targetPosition, progress);
        elapsedTime += Time.unscaledDeltaTime;

        yield return null;
    }

    transform.position = targetPosition;
    newPosition = transform.position;
    isMoving = false;
    }
}
