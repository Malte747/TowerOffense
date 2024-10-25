using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class GridMouseInput : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;

    private UnityEngine.Vector3 lastPos;

    [SerializeField]
    private LayerMask placementLayermask;

    public UnityEngine.Vector3 GetSelectedMapPos()
    {
        UnityEngine.Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, placementLayermask) && hit.collider.CompareTag("GridTag"))
        {
            lastPos = hit.point;
        }
        return lastPos;
        
    }
}
