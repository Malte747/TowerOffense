using UnityEngine;

public class PlaceEnemies : MonoBehaviour
{
    public LayerMask planeLayer;
    public GameObject[] Units;
    public GameObject unit;
    GameObject manager;

    private void Start()
    {
        manager = GameObject.Find("GameManager");
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit, Mathf.Infinity, planeLayer) && unit.GetComponent<EnemyScript>())
            {
                Instantiate(unit, hit.point, Quaternion.identity);
                manager.GetComponent<GameManager>().UnitPayment(unit.GetComponent<EnemyScript>().cost);
            }
        }
    }

    public void ChangeUnit(int unitNumber)
    {
        unit = Units[unitNumber];
    }
}