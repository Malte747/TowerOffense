using UnityEngine;

public class PlaceEnemies : MonoBehaviour
{
    public LayerMask planeLayer;
    public GameObject[] Units;
    public GameObject unit;

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, planeLayer))
            {
                Instantiate(unit, hit.point, Quaternion.identity);
            }
        }
    }

    public void ChangeUnit(int unitNumber)
    {
        unit = Units[unitNumber];
    }
}