using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerGridPlacement : MonoBehaviour
{
    //public List<Vector3Int> Occupied;
    public static Dictionary<Vector3, GameObject> TowerBible = new Dictionary<Vector3, GameObject>();

    public Material indicatorColor;
    public GameObject indicator;
    public static bool placingTowers;

    [SerializeField]
    private Grid grid;

    public int xSize, zSize, placementRangeSX, placementRangeBX, placementRangeSZ, placementRangeBZ;
    public List<GameObject> Towers;
    public List<GameObject> IndicatorTowers;
    private GameObject PlacedTower;

    public static GameObject clickedTowerParent;



    //Bis UI existiert Placeholder
    public static int towerNumberUI;

    void Start()
    {
        indicator.transform.localScale = new Vector3 (xSize, 1, zSize);
        indicator.SetActive(false);
        placingTowers = false;
        clickedTowerParent = gameObject;
    }

    void Update()
    {
        bool hitTower = false;
        for (int i = 1; i <= xSize; i++)
        {

            for (int i2 = 1; i2 <= zSize; i2++)
            {
                Vector3Int gridCheck = new Vector3Int(GridPlacementSystem.gridPosition.x + i - 1, 0, GridPlacementSystem.gridPosition.z + i2 - 1);
                if (TowerBible.ContainsKey(gridCheck))
                {
                    hitTower = true;
                }
            }
        if(GridPlacementSystem.gridPosition.x < placementRangeSX || GridPlacementSystem.gridPosition.x + xSize -1 > placementRangeBX || GridPlacementSystem.gridPosition.z < placementRangeSZ || GridPlacementSystem.gridPosition.z + zSize - 1 > placementRangeBZ)
            {
                hitTower = true;
            }
        }
        if (hitTower)
        {
            //Debug.Log("Position is occupied.");
            indicatorColor.SetColor("_BaseColor", new Color(1f, 0f, 0f, 0.1f));
        }
        else
        {
            //Debug.Log("Position is free.");
            indicatorColor.SetColor("_BaseColor", new Color(0f, 1f, 0f, 0.1f));
        }

        if(EventSystem.current.IsPointerOverGameObject() && placingTowers)
        {
            Cursor.visible = true;
            indicator.transform.parent.gameObject.SetActive(false);
        }
        else if(placingTowers) 
        {
            Cursor.visible = false;
            indicator.transform.parent.gameObject.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if(!EventSystem.current.IsPointerOverGameObject())
            {
            if(!hitTower && placingTowers)
            {
                PlaceTower(towerNumberUI);
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    StopPlacingTowers();
                }
            }
            else if (GridMouseInput.mouseOverTower && !placingTowers)
            {
                ClickOnTower();
            }
            else if (!GridMouseInput.mouseOverTower && !placingTowers)
            {
                UnselectTower();
            }
            }
        }


        if (Input.GetMouseButtonDown(1) && placingTowers)
        {    
            StopPlacingTowers();
        }

    }

    public void PlaceTower(int number)
    {
        // OccupyCell(GridPlacementSystem.gridPosition);
        PlacedTower = Instantiate(Towers[number], grid.CellToWorld(GridPlacementSystem.gridPosition), Quaternion.identity);
        TowerKnowsWhereItIs towerKnowsWhereItIs = PlacedTower.GetComponent<TowerKnowsWhereItIs>();

       GameObject NavMesh = GameObject.Find("NavMesh");
       if(NavMesh != null)
       { 
            NavMeshBaking baking = NavMesh.GetComponent<NavMeshBaking>();
            baking.StartCoroutine("BakeNavMesh");
       }
    
        for (int i = 1; i <= xSize; i++)
        {

            for (int i2 = 1; i2 <= zSize; i2++)
            {
                    OccupyCell(new Vector3Int(GridPlacementSystem.gridPosition.x + i -1, 0 , GridPlacementSystem.gridPosition.z + i2 - 1));
                    towerKnowsWhereItIs.MyCells.Add(new Vector3Int(GridPlacementSystem.gridPosition.x + i -1, 0 , GridPlacementSystem.gridPosition.z + i2 - 1));
                    //Debug.Log("Occupy");
            }
        }
    }

    public void OccupyCell(Vector3Int cellNumbers)
    {
        TowerBible.Add(cellNumbers, PlacedTower);
        
    }

    public void ChangeTowerWhilePlacing(string numbers)
    {
        indicator.SetActive(true);
        Cursor.visible = false;
        placingTowers = true;
        string[] digits = Regex.Split(numbers, @"\D+");
        List<int> ints = new List<int>();
        foreach (string value in digits)
        {
            int number;
            if (int.TryParse(value, out number))
            {
                ints.Add(number);
            }
        }

        towerNumberUI = ints[0];
        xSize = ints[1];
        zSize = ints[2];
        indicator.transform.localScale = new Vector3(xSize, 1, zSize);

        DeactivateTowerPreview();
        IndicatorTowers[ints[0]].SetActive(true);

    }

    public void StopPlacingTowers()
    {
        Cursor.visible = true;
        placingTowers = false;
        DeactivateTowerPreview();
        indicator.SetActive(false);
    }

    public void DeactivateTowerPreview()
    {
        foreach (GameObject tower in IndicatorTowers)
        {
            tower.SetActive(false);
        }
    }

    public void ClickOnTower()
    {
        UnselectTower();
        TowerKnowsWhereItIs towerKnowsWhereItIs = GridMouseInput.clickedTower.GetComponent<TowerKnowsWhereItIs>();
        if(towerKnowsWhereItIs == null) towerKnowsWhereItIs = GridMouseInput.clickedTower.GetComponentInParent<TowerKnowsWhereItIs>();
        //Debug.Log("Cells: " + towerKnowsWhereItIs.MyCells.Count);
        clickedTowerParent = GridMouseInput.clickedTower.transform.parent.gameObject;
        if (clickedTowerParent.GetComponent<Outline>() != null)
        {
            clickedTowerParent.GetComponent<Outline>().enabled = true;
        }
        else
        {
            Outline outline = clickedTowerParent.AddComponent<Outline>();
            outline.enabled = true;
            clickedTowerParent.GetComponent<Outline>().OutlineWidth = 3.0f;
        }
    }

    public void UnselectTower()
    {
        if (clickedTowerParent != null)
        {
            if (clickedTowerParent.GetComponent<Outline>() != null)
            {
                clickedTowerParent.GetComponent<Outline>().enabled = false;
            }
        }
    }
}
