using System.Collections.Generic;
using UnityEngine;

public class PlaceEnemies : MonoBehaviour
{
    public LayerMask planeLayer;
    public GameObject[] Units;
    public GameObject unit;
    GameManager manager;
    bool placingUnit;
    public List<GameObject> indicatorUnits = new List<GameObject>();
    public Material indicatorColor;
    public GameObject indicatorEmpty;
    private UIManager _uiManager;

    private void Start()
    {
        placingUnit = false;
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();

        _uiManager = GameObject.Find("UiManager").GetComponent<UIManager>();
    }
    void Update()
    {
        if(!manager.attackersTurn)
        {
            foreach (GameObject unit in indicatorUnits)
            {
                unit.SetActive(false);
            }
            placingUnit = false;
        }
        if (placingUnit)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, 1000, planeLayer) 
                    && unit.GetComponent<EnemyScript>().supplyCost + manager.attackerSupply <= manager.maxSupply
                    && manager.attackersTurn && unit.GetComponent<EnemyScript>().cost <= manager.attackerGold)
            {
                indicatorEmpty.transform.position = hit.point;
                indicatorColor.SetColor("_BaseColor", new Color(0.09215922f, 0.838f, 0.04049486f, 0.5f));
                
                    if (Input.GetMouseButtonDown(0) && !EnemyBibleScript.EnemyBible.ContainsKey(hit.point))
                    {
                        Instantiate(unit, hit.point, Quaternion.identity);
                        manager.UnitPayment(unit.GetComponent<EnemyScript>().cost);
                        manager.UnitSupplyPayment(unit.GetComponent<EnemyScript>().supplyCost);
                        if (!Input.GetKey(KeyCode.LeftShift))
                        {
                            placingUnit = false;
                            foreach (GameObject unit in indicatorUnits)
                            {
                                unit.SetActive(false);
                            }
                        }

                    }
                
                
                    
                
                
                
            }
            else if (Physics.Raycast(ray, out hit, 1000))
            {
                indicatorColor.SetColor("_BaseColor", new Color(0.8392157f, 0.03921568f, 0.06320632f, 0.5f));
                indicatorEmpty.transform.position = hit.point;

                if (Input.GetMouseButtonDown(0))
                {
                    if (manager.attackersTurn && unit.GetComponent<EnemyScript>().cost > manager.attackerGold)
                    {
                        _uiManager.NotEnoughGoldMessage();
                    }
                    if(unit.GetComponent<EnemyScript>().supplyCost + manager.attackerSupply > manager.maxSupply)
                    {
                        _uiManager.NotEnoughSupplyMessage();
                    }
                    if(!Physics.Raycast(ray, out hit, 1000, planeLayer))
                    {
                        _uiManager.CannotBuildHereMessage();
                    }
                    
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (manager.attackersTurn && unit.GetComponent<EnemyScript>().cost > manager.attackerGold)
                    {
                        _uiManager.NotEnoughGoldMessage();
                    }
                    if (unit.GetComponent<EnemyScript>().supplyCost + manager.attackerSupply > manager.maxSupply)
                    {
                        _uiManager.NotEnoughSupplyMessage();
                    }
                    if (!Physics.Raycast(ray, out hit, 1000, planeLayer))
                    {
                        _uiManager.CannotBuildHereMessage();
                    }
                }
            }
        }
        /*
        if (Input.GetMouseButtonDown(0))
        {



            if (Physics.Raycast(ray, out hit, Mathf.Infinity, planeLayer)
                && unit.GetComponent<EnemyScript>().cost <= manager.attackerGold
                && unit.GetComponent<EnemyScript>().supplyCost + manager.attackerSupply <= manager.maxSupply)
            {
                Instantiate(unit, hit.point, Quaternion.identity);
                manager.UnitPayment(unit.GetComponent<EnemyScript>().cost);
                manager.UnitSupplyPayment(unit.GetComponent<EnemyScript>().supplyCost);
            }
        }
        */
    }

    public void ChangeUnit(int unitNumber)
    {
        unit = Units[unitNumber];
        placingUnit = true;
        foreach (GameObject unit in indicatorUnits)
        {
            unit.SetActive(false);
        }
        indicatorUnits[unitNumber].SetActive(true);
        Cursor.visible = false;
    }

    public void ResetGameUnits()
    {
        Debug.Log("reset units");
        foreach (Vector3 pos in EnemyBibleScript.EnemyBible.Keys)
        {
            Destroy(EnemyBibleScript.EnemyBible[pos]);
        }
        EnemyBibleScript.EnemyBible.Clear();
    }
}