using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        if (attackerHasWon) 
        {
            AttackerHasWon();
        }
    }

    private void AttackerHasWon()
    {
        attackerWinsText.SetActive(true);
        //Debug.Log("Atacker Wins!!!");
        if (Time.timeScale > 0.01f) Time.timeScale -= 0.003f;
        else
        {
            Time.timeScale = 1f;
            attackerHasWon = false;
            SceneManager.LoadScene("Main");
        }
    }
}
