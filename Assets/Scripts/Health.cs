using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

//  SCRIPT SUMMARY: keeps track of a towers/units health & kills it

public class Health : MonoBehaviour
{
    public float health;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        if (gameObject.CompareTag("MainTower"))
        {
            GridPlacementSystem.attackerHasWon = true;
            Destroy(gameObject);
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
        if (gameObject.CompareTag("Tower") || gameObject.CompareTag("Mine") || gameObject.CompareTag("Wall"))
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
                Debug.Log("Mine is not ALive");
            }

            // Remove each of those keys from the dictionary
            foreach (var key in keysToRemove)
            {
                TowerGridPlacement.TowerBible.Remove(key);
            }
        }
        /*else if (gameObject.CompareTag("Enemy"))
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
        }*/

    }
}
