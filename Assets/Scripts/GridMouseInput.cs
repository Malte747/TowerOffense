using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class GridMouseInput : MonoBehaviour
{
    public static bool mouseOverTower;

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
        if (Physics.Raycast(ray, out hit, 1000, placementLayermask))
        {
            if (hit.collider.CompareTag("GridTag"))
            {
                lastPos = hit.point;
                mouseOverTower = false;
            }
            else if (hit.collider.CompareTag("Tower"))
            {
                lastPos = hit.point + new UnityEngine.Vector3(0, 1f, 0);
                mouseOverTower = true;
            }
        }
        else
        {
            lastPos = new UnityEngine.Vector3(0, 0, 300);
            mouseOverTower = false;
        }
        return lastPos;
        
    }
}
