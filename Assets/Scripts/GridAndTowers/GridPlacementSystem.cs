using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridPlacementSystem : MonoBehaviour
{
    public static Vector3Int gridPosition;

    public static bool attackerHasWon = false;
    [SerializeField] public GameObject attackerWinsText;

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
        cellIndicator.transform.position = grid.CellToWorld(gridPosition + TowerGridPlacement.towerRotationCorrection);
        cellIndicator.transform.eulerAngles = new Vector3(cellIndicator.transform.eulerAngles.x, TowerGridPlacement.towerRotation, cellIndicator.transform.eulerAngles.z);
        rotationSave = cellIndicator.transform.rotation;
        //Debug.Log(cellIndicator.transform.position);

        //if (attackerHasWon) 
        {
            //AttackerHasWon();
            //Debug.Log("Atacker Wins!!!");
        }
    }
}
