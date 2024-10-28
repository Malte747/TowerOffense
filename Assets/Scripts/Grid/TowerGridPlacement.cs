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
    public List<Vector3Int> Occupied;
    public Material indicatorColor;
    public GameObject indicator;
    private bool placingTowers;

    [SerializeField]
    private Grid grid;

    public int xSize, zSize, placementRangeSX, placementRangeBX, placementRangeSZ, placementRangeBZ;

    public List<GameObject> Towers;
    public List<GameObject> IndicatorTowers;



    //Bis UI existiert Placeholder
    public static int towerNumberUI;

    void Start()
    {
        indicator.transform.localScale = new Vector3 (xSize, 1, zSize);
        indicator.SetActive(false);
        placingTowers = false;
    }

    void Update()
    {
        bool hitTower = false;
        for (int i = 1; i <= xSize; i++)
        {

            for (int i2 = 1; i2 <= zSize; i2++)
            {
                Vector3Int gridCheck = new Vector3Int(GridPlacementSystem.gridPosition.x + i - 1, 0, GridPlacementSystem.gridPosition.z + i2 - 1);
                if (Occupied.Contains(gridCheck))
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

        if (Input.GetMouseButtonDown(0) && !hitTower && placingTowers)
        {
            if(!EventSystem.current.IsPointerOverGameObject())
            {
                PlaceTower(towerNumberUI);
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    StopPlacingTowers();
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

        for (int i = 1; i <= xSize; i++)
        {

            for (int i2 = 1; i2 <= zSize; i2++)
            {
                    OccupyCell(new Vector3Int(GridPlacementSystem.gridPosition.x + i -1, 0 , GridPlacementSystem.gridPosition.z + i2 - 1));
                    //Debug.Log("Occupy");
            }
        }
        Instantiate(Towers[number], grid.CellToWorld(GridPlacementSystem.gridPosition), Quaternion.identity);
    }

    public void OccupyCell(Vector3Int cellNumbers)
    {
        Occupied.Add(cellNumbers);
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
}
