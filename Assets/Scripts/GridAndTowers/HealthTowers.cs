using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//  SCRIPT SUMMARY: keeps track of a towers/units health & kills it

public class HealthTowers : MonoBehaviour
{
    public int health;
    [HideInInspector] public int healthLastCheck;
    [HideInInspector] public List<GameObject> attackedMainTower = new List<GameObject>();

    private TowerHealthBar _towerHealthBar;

    [SerializeField] public TowerStats TowerStats;

    // Start is called before the first frame update
    private void Start()
    {
        TowerStats.health = health;
        healthLastCheck = health;
        _towerHealthBar = GetComponent<TowerHealthBar>();
    }
    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
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
        if (gameObject.CompareTag("MainTower"))
        {
            GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            gameManager.EndGameAttackerWin();
            GridPlacementSystem.attackerHasWon = true;
        }
        else
        { 
            RemoveEntries(gameObject);
            NavMeshBaking baking = GameObject.Find("NavMesh").GetComponent<NavMeshBaking>();
            baking.StartCoroutine("BakeNavMesh");
            Destroy(gameObject);
            
        }
        
    }

    void RemoveEntries(GameObject targetObject)
    {
        var keysToRemove = TowerGridPlacement.TowerBible
        .Where(entry => entry.Value == targetObject)
        .Select(entry => entry.Key)
        .ToList();

        //Remove Gold Income if Mine
        if(gameObject.CompareTag("Mine"))
        {
            Mine mine = gameObject.GetComponent<Mine>();
            mine.MineIsDying();
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