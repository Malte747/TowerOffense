using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class TowerGridPlacement : MonoBehaviour
{
    public static Dictionary<Vector3, GameObject> TowerBible = new Dictionary<Vector3, GameObject>();

    public static Dictionary<Vector3Int, GameObject> cellsWithLingeringAoe = new Dictionary<Vector3Int, GameObject>();

    public Material indicatorColor;
    public GameObject indicator;
    public GameManager gameManager;
    public static bool placingTowers;
    int xSizeDirection, zSizeDirection, xAdjustment, zAdjustment;
    public static int towerRotation;
    public static Vector3Int towerRotationCorrection;
    private UIManager _uiManager;
    private HealthTowers healthTowers;

    [SerializeField] private GameObject MainTowerPrefab;

    TowerKnowsWhereItIs towerKnowsWhereItIs;
    TowerStats towerStats;

    [SerializeField]
    private Grid grid;

    public int xSize, zSize, placementRangeSX, placementRangeBX, placementRangeSZ, placementRangeBZ;
    public List<GameObject> Towers;
    public List<GameObject> IndicatorTowers;
    private GameObject PlacedTower;
    private bool hitTower;
    private GameObject meshes;

    public static GameObject clickedTowerParent;
    public static int towerNumberUI;

    void Start()
    {
        indicator.transform.localScale = new Vector3 (xSize, 1, zSize);
        indicator.SetActive(false);
        placingTowers = false;
        clickedTowerParent = gameObject;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _uiManager = GameObject.Find("UiManager").GetComponent<UIManager>();
        BuildSoundList(); //Liste Für TowerPlaceSFX
    }

    void Update()
    {
        if (!gameManager.defendersTurn) StopPlacingTowers();

        if (healthTowers != null && healthTowers.health < healthTowers.healthLastCheck)
        {
            TowerInfoUIHPChange();
        }


        #region Mouse Inputs

        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (placingTowers)
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

        #endregion


        if (!gameManager.defendersTurn) return;


        #region Mouse Inputs(Only Defenders Turn) 

        if (Input.GetMouseButtonDown(1) && placingTowers)
        {
            StopPlacingTowers();
        }

        if (Input.GetKeyDown(KeyCode.Tab) && placingTowers)
        {
            towerRotation += 90;
            if (towerRotation >= 360) towerRotation = 0;
            if (towerRotation == 0)
            {
                xSize = towerStats.xSize;
                zSize = towerStats.zSize;
                xSizeDirection = +1;
                zSizeDirection = +1;
                xAdjustment = 1;
                zAdjustment = 1;
                towerRotationCorrection = new Vector3Int(0, 0, 0);
            }
        }
        else if (towerRotation == 90)
        {
            xSize = towerStats.zSize;
            zSize = -towerStats.xSize;
            xSizeDirection = 1;
            zSizeDirection = -1;
            xAdjustment = 1;
            zAdjustment = 0;
            towerRotationCorrection = new Vector3Int(0, 0, 1);
        }
        else if (towerRotation == 180)
        {
            xSize = -towerStats.xSize;
            zSize = -towerStats.zSize;
            xSizeDirection = -1;
            zSizeDirection = -1;
            xAdjustment = 0;
            zAdjustment = 0;
            towerRotationCorrection = new Vector3Int(1, 0, 1);
        }
        else if (towerRotation == 270)
        {
            xSize = -towerStats.zSize;
            zSize = towerStats.xSize;
            xSizeDirection = -1;
            zSizeDirection = 1;
            xAdjustment = 0;
            zAdjustment = 1;
            towerRotationCorrection = new Vector3Int(1, 0, 0);
        }

        #endregion


        #region Grid Check 
        hitTower = false;
        for (int i = 1; i <= Mathf.Abs(xSize); i++)
        {

            for (int i2 = 1; i2 <= Mathf.Abs(zSize); i2++)
            {
                Vector3Int gridCheck = new Vector3Int(GridPlacementSystem.gridPosition.x + (i * xSizeDirection) - xAdjustment, 0, GridPlacementSystem.gridPosition.z + (i2 * zSizeDirection) -zAdjustment) + towerRotationCorrection;
                if (TowerBible.ContainsKey(gridCheck))
                {
                    hitTower = true;
                }
                if(gridCheck.x < placementRangeSX || gridCheck.x > placementRangeBX || gridCheck.z < placementRangeSZ || gridCheck.z > placementRangeBZ)
                {
                    hitTower = true;
                }
            }
        }
        if (towerKnowsWhereItIs == null)
        {
            //Debug.Log("No Tower Selected.");
            indicatorColor.SetColor("_BaseColor", new Color(0.8392157f, 0.03921568f, 0.06320632f, 0.5f));
        }
        else if (!hitTower  && towerStats.goldCost  <= gameManager.defenderGold  && towerStats.supplyCost + gameManager.defenderSupply <= gameManager.defenderMaxSupply)
        {
            //Debug.Log("Position is free.");
            indicatorColor.SetColor("_BaseColor", new Color(0.09215922f, 0.838f, 0.04049486f, 0.5f));
        }
        else
        {
            //Debug.Log("Position is occupied.");
            indicatorColor.SetColor("_BaseColor", new Color(0.8392157f, 0.03921568f, 0.06320632f, 0.5f));
        }

        if(!GridMouseInput.mouseOverGrid && placingTowers || EventSystem.current.IsPointerOverGameObject() && placingTowers)
        {
            Cursor.visible = true;
            
            indicator.transform.parent.gameObject.SetActive(false);
        }
        else if(placingTowers) 
        {
            Cursor.visible = false;
            
            indicator.transform.parent.gameObject.SetActive(true);
        }

        #endregion
    }

    #region Place Towers 

    public void PlaceTower(int number)
    {
        towerKnowsWhereItIs = Towers[number].GetComponent<TowerKnowsWhereItIs>();
        towerStats = towerKnowsWhereItIs.TowerStats;
        if (!hitTower && towerStats.goldCost <= gameManager.defenderGold && towerStats.supplyCost + gameManager.defenderSupply <= gameManager.defenderMaxSupply)
        {
            gameManager.TurretPayment(towerStats.goldCost);
            gameManager.TurretSupplyPayment(towerStats.supplyCost);
            // OccupyCell(GridPlacementSystem.gridPosition);
            PlacedTower = Instantiate(Towers[number], grid.CellToWorld(GridPlacementSystem.gridPosition + towerRotationCorrection), GridPlacementSystem.rotationSave);
            towerKnowsWhereItIs = PlacedTower.GetComponent<TowerKnowsWhereItIs>();
            towerStats = towerKnowsWhereItIs.TowerStats;

            //Sound wird abgespielt
            if(TowerPlaceSound.Count > number) towerSFX.PlayTowerSound(TowerPlaceSound[number]); //Spielt die gewünschte SFX Nummer

            GameObject NavMesh = GameObject.Find("NavMesh");
            if (NavMesh != null)
            {
                NavMeshBaking baking = NavMesh.GetComponent<NavMeshBaking>();
                baking.StartCoroutine("BakeNavMesh");
            }

            for (int i = 1; i <= Mathf.Abs(xSize); i++)
            {

                for (int i2 = 1; i2 <= Mathf.Abs(zSize); i2++)
                {
                    OccupyCell(new Vector3Int(GridPlacementSystem.gridPosition.x + (i * xSizeDirection) - xAdjustment, 0, GridPlacementSystem.gridPosition.z + (i2 * zSizeDirection) - zAdjustment) + towerRotationCorrection);
                    towerKnowsWhereItIs.MyCells.Add(new Vector3Int(GridPlacementSystem.gridPosition.x + (i * xSizeDirection) - xAdjustment, 0, GridPlacementSystem.gridPosition.z + (i2 * zSizeDirection) - zAdjustment) + towerRotationCorrection);
                    //Debug.Log("Occupy");
                }
            }
        }
        else if(towerStats.goldCost > gameManager.defenderGold)
        {
            _uiManager.NotEnoughGoldMessage();
        }
        else if (towerStats.supplyCost + gameManager.defenderSupply > gameManager.defenderMaxSupply)
        {
            _uiManager.NotEnoughSupplyMessage();
        }
        else if (hitTower) 
        {
            _uiManager.CannotBuildHereMessage();
        }
    }

    public void OccupyCell(Vector3Int cellNumbers)
    {
        TowerBible.Add(cellNumbers, PlacedTower);
        
    }

    #endregion

    #region Tower Select

    public void ChangeTowerWhilePlacing(int number)
    {
        if(towerKnowsWhereItIs != null) UnselectTower();
        towerRotation = 0;
        xSizeDirection = +1;
        zSizeDirection = +1;
        xAdjustment = 1;
        zAdjustment = 1;
        towerRotationCorrection = new Vector3Int(0, 0, 0);
        indicator.SetActive(true);
        Cursor.visible = false;
        placingTowers = true;
        towerKnowsWhereItIs = Towers[number].GetComponent<TowerKnowsWhereItIs>();
        towerNumberUI = number;
        towerStats = towerKnowsWhereItIs.TowerStats;
        xSize = towerStats.xSize;
        zSize = towerStats.zSize;

        indicator.transform.localScale = new Vector3(xSize, 1, zSize);

        DeactivateTowerPreview();
        IndicatorTowers[number].SetActive(true);

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

    #endregion

    #region Click Towers

    public void ClickOnTower()
    {
        UnselectTower();
        clickedTowerParent = GridMouseInput.clickedTower.transform.root.gameObject;
        towerKnowsWhereItIs = clickedTowerParent.GetComponent<TowerKnowsWhereItIs>();
        towerStats = towerKnowsWhereItIs.TowerStats;
        if (towerKnowsWhereItIs == null) towerKnowsWhereItIs = clickedTowerParent.GetComponentInParent<TowerKnowsWhereItIs>();
        Debug.Log("Cells: " + towerKnowsWhereItIs.MyCells.Count);
        healthTowers = clickedTowerParent.GetComponent<HealthTowers>();
        TowerInfoUI();
        meshes = clickedTowerParent.transform.GetChild(0).gameObject;
        if (meshes.transform.GetChild(0).gameObject.name == "Moving Details") meshes = meshes.transform.GetChild(1).gameObject;
        if (meshes.GetComponent<Outline>() != null)
        {
            meshes.GetComponent<Outline>().enabled = true;
        }
        else
        {
            Outline outline = meshes.AddComponent<Outline>();
            outline.enabled = true;
            meshes.GetComponent<Outline>().OutlineWidth = towerStats.outlineSize;
        }
        MeshRenderer towerBoden = towerKnowsWhereItIs.gameObject.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>();
        towerBoden.renderingLayerMask = 2;
    }

    public void UnselectTower()
    {
        if (clickedTowerParent != null && meshes != null)
        {
            if (meshes.GetComponent<Outline>() != null)
            {
                meshes.GetComponent<Outline>().enabled = false;
            }
            _uiManager.HideTowerInfoUI();
            if (towerKnowsWhereItIs != null) 
            { 
                MeshRenderer towerBoden = towerKnowsWhereItIs.gameObject.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>();
                towerBoden.renderingLayerMask = 0;
            }
        }
    }

    public void TowerInfoUI()
    {
        TowerStats towerstats = healthTowers.TowerStats;
        _uiManager.ShowTowerInfoUI();
        TowerInfoUIHPChange();
        if (towerKnowsWhereItIs != null) _uiManager.SetTowerRepairCost(towerStats.goldCost, towerstats.health, healthTowers.health);
        //Bild Change
    }

    public void TowerInfoUIHPChange()
    {
        TowerStats towerstats = healthTowers.TowerStats;
        _uiManager.SetTowerHPSliderUIValues(towerstats.health, healthTowers.health);
    }

    public void TowerUIButtons(bool repair)
    {
        if (healthTowers != null && repair)
        {
            if (UIManager.towerRepairCost <= gameManager.defenderGold)
            {
                gameManager.TurretPayment(UIManager.towerRepairCost);
                healthTowers.RepairTower();
            }
        }
        else if (healthTowers != null && !repair) 
        { 
            healthTowers.Death();
        }
    }

    #endregion

    #region ResetGame

    public void ResetGameTowers()
    {
        foreach (Vector3 pos in TowerBible.Keys) 
        {
            Destroy(TowerBible[pos]);
        }
        TowerBible.Clear();
        GameObject main = GameObject.Find("MainTower");
        Destroy(main);
        Instantiate(MainTowerPrefab, new Vector3(0, 20, 145), Quaternion.identity);
    }


    #endregion

    #region Sound
    private AudioManager towerSFX; // Referenz zum AudioManager Script 

    public  List <int> TowerPlaceSound;

    private void BuildSoundList()
    {
        towerSFX = GameObject.Find("AudioManager").GetComponent<AudioManager>(); //Nimmt den Audio Manager in das Script

        //Platziert eine Listen Platz für TowerIndex Nummer mit passendem Sound im Audiomanager.
        TowerPlaceSound.Add(6);
        TowerPlaceSound.Add(3);
        TowerPlaceSound.Add(10);
        TowerPlaceSound.Add(7);
        TowerPlaceSound.Add(4);
        TowerPlaceSound.Add(11);
        TowerPlaceSound.Add(14);
        TowerPlaceSound.Add(16);
        TowerPlaceSound.Add(17);
        TowerPlaceSound.Add(18);
        TowerPlaceSound.Add(5);
    }
    #endregion
}
