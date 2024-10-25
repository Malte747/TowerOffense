using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseIndicator, cellIndicator;

    [SerializeField]
    private GridMouseInput gridMouseInput;

    [SerializeField]
    private Grid grid;
    private void Update()
    {
        Vector3 mousePos = gridMouseInput.GetSelectedMapPos();
        Vector3Int gridPosition = grid.WorldToCell(mousePos);
        mouseIndicator.transform.position = mousePos;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }
}
