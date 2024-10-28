using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class GridMouseInput : MonoBehaviour
{
    public static bool mouseOverTower, mouseOverGrid;

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
                mouseOverGrid = true;
                mouseOverTower = false;
            }
            else if (hit.collider.CompareTag("Tower"))
            {
                lastPos = hit.point + new UnityEngine.Vector3(0, 0.5f, 0);
                mouseOverGrid = true;
                mouseOverTower = true;
            }
            else
            {
            lastPos = new UnityEngine.Vector3(hit.point.x , 0, hit.point.z);
            mouseOverGrid = false;
            mouseOverTower = false;
            }
        }
        else        
        {
            
        }
        return lastPos;
        
    }
}
