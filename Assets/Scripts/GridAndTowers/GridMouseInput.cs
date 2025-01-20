using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GridMouseInput : MonoBehaviour
{
    public static bool mouseOverTower, mouseOverGrid, gridBehindTower;   

    [SerializeField]
    private Camera sceneCamera;

    private Vector3 lastPos;
    public static GameObject clickedTower;

    [SerializeField]
    private LayerMask placementLayermask, onlyGridLayermask;

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
                //Debug.Log("HitGrid");
                lastPos = new Vector3(hit.point.x , 0.0f, hit.point.z);
                mouseOverGrid = true;
                mouseOverTower = false;
                gridBehindTower = true;
            }
            else if (hit.collider.CompareTag("Tower") || hit.collider.CompareTag("Mine") || hit.collider.CompareTag("Wall") || hit.collider.CompareTag("MainTower") || hit.collider.CompareTag("SupplyHouse") || hit.collider.CompareTag("LingeringDamage"))
            {
                //Debug.Log("HitTower");
                lastPos = new Vector3(hit.point.x , 0.0f, hit.point.z);
                mouseOverGrid = true;
                mouseOverTower = true;
                if (hit.collider != null)
                {
                        clickedTower = hit.collider.gameObject;
                }
                Ray ray2 = sceneCamera.ScreenPointToRay(mousePos);
                RaycastHit hit2;
                if (Physics.Raycast(ray2, out hit2, 1000, onlyGridLayermask))
                {
                    gridBehindTower = true;
                    lastPos = new Vector3(hit2.point.x , 0.0f, hit2.point.z);   
                }
            }
            else
            {
                //Debug.Log("HitElse");
                lastPos = new Vector3(hit.point.x , 0, hit.point.z);
                mouseOverGrid = false;
                mouseOverTower = false;
                gridBehindTower = false;
            }  
        }
        else
        {
            //Debug.Log("HitNothing");
            mouseOverGrid = false;
            mouseOverTower = false;
            gridBehindTower = false;
        }
        //Debug.Log(lastPos);
        return lastPos;
        
    }
}
