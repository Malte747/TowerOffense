using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GridMouseInput : MonoBehaviour
{
    public static bool mouseOverTower, mouseOverGrid;

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
            if (hit.collider.CompareTag("GridTag"))
            {
                lastPos = hit.point;
                mouseOverGrid = true;
                mouseOverTower = false;
            }
            else if (hit.collider.CompareTag("Tower"))
            {
                lastPos = hit.point + new Vector3(0, 0.5f, 0);
                mouseOverGrid = true;
                mouseOverTower = true;
            }
            else
            {
            lastPos = new Vector3(hit.point.x , 0, hit.point.z);
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
