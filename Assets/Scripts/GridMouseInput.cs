using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMouseInput : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;

    private Vector3 lastPos;

    [SerializeField]
    private LayerMask placementLayermask;

    public Vector3 GetSelectedMapPos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, placementLayermask))
        {
            lastPos = hit.point;
        }
        return lastPos;
    }
}
