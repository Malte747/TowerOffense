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

    // Start is called before the first frame update
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        maxHealth = health;
        healthLastCheck = health;
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
