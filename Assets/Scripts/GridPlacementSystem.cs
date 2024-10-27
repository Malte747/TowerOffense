using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlacementSystem : MonoBehaviour
{
    public static Vector3Int gridPosition; 

    [SerializeField]
    private GameObject mouseIndicator, cellIndicator;

    [SerializeField]
    private GridMouseInput gridMouseInput;

    [SerializeField]
    private Grid grid;
    private void Update()
    {
        Vector3 mousePos = gridMouseInput.GetSelectedMapPos();
        gridPosition = grid.WorldToCell(mousePos);
        //Debug.Log(grid.WorldToCell(mousePos));
        mouseIndicator.transform.position = mousePos;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }
}
