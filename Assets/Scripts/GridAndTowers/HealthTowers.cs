using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//  SCRIPT SUMMARY: keeps track of a towers/units health & kills it

public class HealthTowers : MonoBehaviour
{
    public int health;
    private bool hasDied = false;
    [HideInInspector] public int healthLastCheck;
    [HideInInspector] public List<GameObject> attackedMainTower = new List<GameObject>();
    public GameObject deathObject;

    private TowerHealthBar _towerHealthBar;

    [SerializeField] public TowerStats TowerStats;

    private GameManager gameManager;
    private AudioManager towerSFX; // Referenz zum AudioManager Script 

    // Start is called before the first frame update
    private void Start()
    {
        health = TowerStats.health;
        healthLastCheck = health;
        _towerHealthBar = GetComponent<TowerHealthBar>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        towerSFX = GameObject.Find("AudioManager").GetComponent<AudioManager>(); //Nimmt den Audio Manager in das Script
    }
    // Update is called once per frame
    void Update()
    {
        if (health <= 0 && !hasDied)
        {
            Death();
        }
        else if (health < healthLastCheck && _towerHealthBar != null)
        {
            healthLastCheck = health;
            _towerHealthBar.UpdateHealthBar(TowerStats.health, health);
        }
    }

    public void Death()
    {
        if (hasDied) return;
        hasDied = true;
        StartCoroutine("DeathAnimation");

        //An der stelle, an welcher SFX ausgelöst werden sollen platzieren
        towerSFX.PlayTowerSound(1); //Spielt die gewünschte SFX Nummer

        //Unselect Tower in UI upon Death
        TowerGridPlacement towerGridPlacement = GameObject.Find("TowerGridPlacement").GetComponent<TowerGridPlacement>();
        if (towerGridPlacement == null) Debug.LogError("Couldn't find TowerGridPlacement to Unselect Tower in UI upon Death");
        else towerGridPlacement.UnselectTower();

        if (gameObject.CompareTag("MainTower"))
        {
            gameManager.EndGameAttackerWin();
            GridPlacementSystem.attackerHasWon = true;
        }
        else
        {
            RemoveEntries(gameObject);
            NavMeshBaking baking = GameObject.Find("NavMesh").GetComponent<NavMeshBaking>();
            baking.StartCoroutine("BakeNavMesh");
            gameManager.TurretSupplyPayment(-TowerStats.supplyCost);
            if (deathObject != null)
            { 
                GameObject spawnedDeathObject = Instantiate(deathObject, transform.position, Quaternion.identity);
                spawnedDeathObject.transform.localScale = new Vector3(TowerStats.xSize, 1, TowerStats.zSize);
            }
            Destroy(gameObject);
        }
        
    }

    IEnumerator DeathAnimation()
    {
        
        yield return null;
    }

    void RemoveEntries(GameObject targetObject)
    {
        var keysToRemove = TowerGridPlacement.TowerBible
        .Where(entry => entry.Value == targetObject)
        .Select(entry => entry.Key)
        .ToList();

        //Remove Supply if House
        if(gameObject.CompareTag("SupplyHouse"))
        {
             SupplyHouse supplyHouse = gameObject.GetComponent<SupplyHouse>();
             gameManager.GainMaxSupplyDefender(-TowerStats.supplyProduced);
        }

        // Remove each of those keys from the dictionary
        foreach (var key in keysToRemove)
        {
            TowerGridPlacement.TowerBible.Remove(key);
        }
        

    }
    public void RepairTower()
    {
        health = TowerStats.health;
        healthLastCheck = TowerStats.health;
        _towerHealthBar.UpdateHealthBar(TowerStats.health, health);
    }
}
