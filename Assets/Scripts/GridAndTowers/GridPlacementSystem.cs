using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlacementSystem : MonoBehaviour
{
    public static Vector3Int gridPosition; 

    [SerializeField]
    private GameObject cellIndicator;
    public static Quaternion rotationSave;

    [SerializeField]
    private GridMouseInput gridMouseInput;

    [SerializeField]
    private Grid grid;
    private void Update()
    {
        Vector3 mousePos = gridMouseInput.GetSelectedMapPos();
        gridPosition = grid.WorldToCell(mousePos);
        //Debug.Log(grid.WorldToCell(mousePos));  
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
        cellIndicator.transform.eulerAngles = new Vector3(cellIndicator.transform.eulerAngles.x, TowerGridPlacement.towerRotation, cellIndicator.transform.eulerAngles.z);
        rotationSave = cellIndicator.transform.rotation;
        /*
        Transform child1 = cellIndicator.transform.GetChild(0);
        Transform child2 = cellIndicator.transform.GetChild(1);

        child1.rotation = Quaternion.Euler(0, TowerGridPlacement.towerRotation, 0);
        child2.rotation = Quaternion.Euler(0, TowerGridPlacement.towerRotation, 0);
        rotationSave = child1.transform.rotation;
        */
    }
}