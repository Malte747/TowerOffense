using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GridMouseInput : MonoBehaviour
{
    public static bool mouseOverTower, mouseOverGrid;   

    [SerializeField]
    private Camera sceneCamera;

    private Vector3 lastPos;
    public static GameObject clickedTower;

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
                lastPos = new Vector3(hit.point.x , 0.0f, hit.point.z);
                mouseOverGrid = true;
                mouseOverTower = false;
            }
            else if (hit.collider.CompareTag("Tower") || hit.collider.CompareTag("Mine") || hit.collider.CompareTag("Wall") || hit.collider.CompareTag("MainTower"))
            {
                lastPos = new Vector3(hit.point.x , 0.0f, hit.point.z);
                mouseOverGrid = true;
                mouseOverTower = true;
                 if (hit.collider != null)
                    {
                        clickedTower = hit.collider.gameObject;
                    }
            }
            else
            {
            lastPos = new Vector3(hit.point.x , 0, hit.point.z);
            mouseOverGrid = false;
            mouseOverTower = false;
            }  
        }
        return lastPos;
        
    }
}
