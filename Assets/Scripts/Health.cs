using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//  SCRIPT SUMMARY: keeps track of a towers/units health & kills it

public class Health : MonoBehaviour
{
    GameManager gameManager;
    public int health;
    [HideInInspector] public int maxHealth;
    [HideInInspector] public int healthLastCheck;
    [HideInInspector] public List<GameObject> attackedMainTower = new List<GameObject>();

    Animator animator;
    bool damn;

    private AudioManager unitSFX; // Referenz zum AudioManager Script
    [SerializeField] private int deathSoundNumber1,deathSoundNumber2;
    private bool deathSoundPlayed = false;

    // Start is called before the first frame update
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        unitSFX = GameObject.Find("AudioManager").GetComponent<AudioManager>(); //Nimmt den Audio Manager in das Script

        maxHealth = health;
        healthLastCheck = health;
        damn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        RemoveEntries(gameObject);
        animator = transform.GetChild(1).GetComponent<Animator>();
        GetComponent<EnemyScript>().enabled = false;
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        animator.SetTrigger("death");
        HealthTowers towerHealth = GameObject.Find("MainTower Prefab(Clone)").GetComponent<HealthTowers>();
        towerHealth.attackedMainTower.Remove(gameObject);
        gameManager.CheckIfTurnIsOver();
        if(!damn)
        {
            GameObject.Find("EnemyPlacementPlane").GetComponent<PlaceEnemies>().combinedIncomePerSec -= GetComponent<EnemyScript>().incomePerSec;
            damn = true;
        }

        if (deathSoundPlayed == false)
        {
        unitSFX.PlayUnitSound(Random.Range(deathSoundNumber1,deathSoundNumber2)); //Spielt die gew�nschte SFX Nummer
        deathSoundPlayed = true;
        }

        Destroy(gameObject, 3.6f);             
    }

    void RemoveEntries(GameObject targetObject)
    {
         var keysToRemove = EnemyBibleScript.EnemyBible
        .Where(entry => entry.Value == targetObject)
        .Select(entry => entry.Key)
        .ToList();

        // Remove each of those keys from the dictionary
        foreach (var key in keysToRemove)
        {
             EnemyBibleScript.EnemyBible.Remove(key);
        }      
    }
}
