using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerGridPlacement : MonoBehaviour
{
    public List<Vector3Int> Occupied;
    public Material indicatorColor;

    
    public static bool t1x2, t2x1, t2x2;
    public static int xSize, zSize;

    void Start()
    {
        t1x2 = true;
        t2x1 = true;
        t2x2 = true;

        xSize = 5;
        zSize = 5;
    }

    void Update()
    {
        bool redq = false;
        for (int i = 1; i <= xSize; i++)
        {

            for (int i2 = 1; i2 <= zSize; i2++)
            {
                Vector3Int gridCheck = new Vector3Int(GridPlacementSystem.gridPosition.x + i - 1, 0, GridPlacementSystem.gridPosition.z + i2 - 1);
                if (Occupied.Contains(gridCheck))
                {
                    redq = true;
                }
            }
        }
        if (redq)
        {
            Debug.Log("Position is occupied.");
            indicatorColor.SetColor("_BaseColor", Color.red);
        }
        else
        {
            //Debug.Log("Position is free.");
            indicatorColor.SetColor("_BaseColor", Color.green);
        }

        
    }

    public void OccupyCell(Vector3Int cellNumbers)
    {
        Occupied.Add(cellNumbers);
    }
}
