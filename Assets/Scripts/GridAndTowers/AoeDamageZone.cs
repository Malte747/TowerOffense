using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeDamageZone : MonoBehaviour
{
    public TowerStats towerStats;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(Damage), 0,towerStats.attackCooldown);
    }

    // Update is called once per frame

    void Damage()
    {
        bool didDamage = false;
        foreach (Vector3 pos in EnemyBibleScript.EnemyBible.Keys)
        {
            if(Vector3.Distance(transform.position, pos) <= towerStats.attackRange) 
            {
                if (EnemyBibleScript.EnemyBible.ContainsKey(pos)) 
                { 
                    GameObject nextVictim = EnemyBibleScript.EnemyBible[pos];
                    Health health = nextVictim.GetComponent<Health>();
                    health.health -= towerStats.damage;
                    didDamage = true;

                }
            }

        }
        if (didDamage)
        {
            //Sound
        }
    }
}
