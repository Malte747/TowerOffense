using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceEnemies : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;

    private Vector3 placePos;

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
                placePos = hit.point;
            }
            else if (hit.collider.CompareTag("Tower"))
            {
                placePos = hit.point;
            }
            else
            {
                placePos = new Vector3(hit.point.x, 0, hit.point.z);
            }
        }
        else
        {

        }
        return placePos;

    }
}
