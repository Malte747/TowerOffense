using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        RemoveEntries(gameObject);
        Destroy(gameObject);
        
    }

    void RemoveEntries(GameObject targetObject)
    {
        if (gameObject.CompareTag("Tower") || gameObject.CompareTag("Mine") || gameObject.CompareTag("Wall"))
        {
            var keysToRemove = TowerGridPlacement.TowerBible
            .Where(entry => entry.Value == targetObject)
            .Select(entry => entry.Key)
            .ToList();

            // Remove each of those keys from the dictionary
            foreach (var key in keysToRemove)
            {
                TowerGridPlacement.TowerBible.Remove(key);
            }
        }
        else if (gameObject.CompareTag("Enemy"))
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
}