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
    private float placeTimer, t2 = 0;
    public int combinedIncomePerSec = 0;
    float holdTime = 0;


    private void Start()
    {
        placingUnit = false;
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();

        _uiManager = GameObject.Find("UiManager").GetComponent<UIManager>();
        combinedIncomePerSec = 0;
    }
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            holdTime += Time.unscaledDeltaTime;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            holdTime = 0;
        }
        t2 += Time.deltaTime;
        if (t2>=3)
        {
            t2 = 0;
            if(manager.attackersTurn)
            {
            manager.GainIncomeAttacker(combinedIncomePerSec);
            }
        }
        if(!manager.attackersTurn) //Deactivates placing because Attackers Turn
        {
            StopPlacingUnits();
        }
        if (placingUnit) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, 1000, planeLayer) 
                    && unit.GetComponent<EnemyScript>().supplyCost + manager.attackerSupply <= manager.attackerMaxSupply
                    && manager.attackersTurn && unit.GetComponent<EnemyScript>().cost <= manager.attackerGold) //Check is Unit is allowed to be placed
            {
                indicatorEmpty.transform.position = hit.point;
                indicatorColor.SetColor("_BaseColor", new Color(0.09215922f, 0.838f, 0.04049486f, 0.5f));

                if (Input.GetMouseButton(0) && Time.unscaledTime > placeTimer + 0.1f && (holdTime < 0.2f || holdTime >= 0.5f))
                {
                    placeTimer = Time.unscaledTime;
                    Instantiate(unit, hit.point, Quaternion.identity);
                    manager.UnitPayment(unit.GetComponent<EnemyScript>().cost);
                    manager.UnitSupplyPayment(unit.GetComponent<EnemyScript>().supplyCost);
                    if (!Input.GetKey(KeyCode.LeftShift))
                    {
                        StopPlacingUnits();
                    }

                }
                else if (Input.GetMouseButtonDown(1))
                {
                    StopPlacingUnits();
                }





            }
            else if (Physics.Raycast(ray, out hit, 1000)) //Raycast hits something other than UnitPlacementLayer
            {
                indicatorColor.SetColor("_BaseColor", new Color(0.8392157f, 0.03921568f, 0.06320632f, 0.5f));
                indicatorEmpty.transform.position = hit.point;

                if (Input.GetMouseButtonDown(0))
                {
                    if (manager.attackersTurn && unit.GetComponent<EnemyScript>().cost > manager.attackerGold)
                    {
                        _uiManager.NotEnoughGoldMessage();
                    }
                    if(unit.GetComponent<EnemyScript>().supplyCost + manager.attackerSupply > manager.attackerMaxSupply)
                    {
                        _uiManager.NotEnoughSupplyMessage();
                    }
                    if(!Physics.Raycast(ray, out hit, 1000, planeLayer))
                    {
                        _uiManager.CannotBuildHereMessage();
                    }
                    
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    StopPlacingUnits();
                }
            }
            else if (!manager.attackersTurn)
            {
                placingUnit = false;
                foreach (GameObject unit in indicatorUnits)
                {
                    unit.SetActive(false);
                }
            }
            else //Raycast hits nothing -> Error Messages
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (manager.attackersTurn && unit.GetComponent<EnemyScript>().cost > manager.attackerGold)
                    {
                        _uiManager.NotEnoughGoldMessage();
                    }
                    if (unit.GetComponent<EnemyScript>().supplyCost + manager.attackerSupply > manager.attackerMaxSupply)
                    {
                        _uiManager.NotEnoughSupplyMessage();
                    }
                    if (!Physics.Raycast(ray, out hit, 1000, planeLayer))
                    {
                        _uiManager.CannotBuildHereMessage();
                    }
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    StopPlacingUnits();
                }
            }
        }
        /*
        if (Input.GetMouseButtonDown(0))
        {



            if (Physics.Raycast(ray, out hit, Mathf.Infinity, planeLayer)
                && unit.GetComponent<EnemyScript>().cost <= manager.attackerGold
                && unit.GetComponent<EnemyScript>().supplyCost + manager.attackerSupply <= manager.attackerMaxSupply)
            {
                Instantiate(unit, hit.point, Quaternion.identity);
                manager.UnitPayment(unit.GetComponent<EnemyScript>().cost);
                manager.UnitSupplyPayment(unit.GetComponent<EnemyScript>().supplyCost);
            }
        }
        */
    }

    public void StopPlacingUnits() //Info: Wird vom Button aus gecalled wenn die Upgrade Karten gecalled werden
    {
        placingUnit = false;
        foreach (GameObject unit in indicatorUnits)
        {
            unit.SetActive(false);
        }
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