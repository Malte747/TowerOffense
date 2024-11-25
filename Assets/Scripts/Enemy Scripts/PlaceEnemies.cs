using UnityEngine;

public class PlaceEnemies : MonoBehaviour
{
    public LayerMask planeLayer;
    public GameObject[] Units;
    public GameObject unit;
    GameManager manager;

    private void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit, Mathf.Infinity, planeLayer)
                && unit.GetComponent<EnemyScript>().cost <= manager.attackerGold
                && unit.GetComponent<EnemyScript>().supplyCost + manager.attackerSupply <= manager.maxSupply)
            {
                Instantiate(unit, hit.point, Quaternion.identity);
                manager.UnitPayment(unit.GetComponent<EnemyScript>().cost);
                manager.UnitSupplyPayment(unit.GetComponent<EnemyScript>().supplyCost);
            }
        }
    }

    public void ChangeUnit(int unitNumber)
    {
        unit = Units[unitNumber];
    }
}