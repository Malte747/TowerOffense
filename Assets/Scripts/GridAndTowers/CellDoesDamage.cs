using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellDoesDamage : MonoBehaviour
{
    private Grid grid;
    [HideInInspector] public Vector3 fieldPosition;
    private Vector3Int cellPosition;
    [HideInInspector] public float duration;
    [HideInInspector] public TowerStats towerStats;


    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        cellPosition = grid.WorldToCell(transform.position);
        if (towerStats.lingeringAoe)
        {
            if (TowerGridPlacement.cellsWithLingeringAoe.ContainsKey(fieldPosition) && towerStats.target != TowerStats.Targets.CellDoesDamage) //If there would be 2 lingeringAoe on the same cell the one with lower damage gets deleted
            {
                Debug.Log("a");
                TowerGridPlacement.cellsWithLingeringAoe.TryGetValue(fieldPosition, out GameObject otherAoe);
                TowerStats otherAoeStats = otherAoe.GetComponent<HealthTowers>().TowerStats;
                if (otherAoe.GetComponent<CellDoesDamage>().duration * otherAoeStats.lingeringAoeDamage > duration * towerStats.lingeringAoeDamage) Destroy(gameObject);
                else
                { 
                    Destroy(otherAoe);
                    TowerGridPlacement.cellsWithLingeringAoe.Add(fieldPosition, gameObject);
                }
            }
        }
        if (!towerStats.lingeringAoe) duration = Mathf.Infinity;
        else duration = towerStats.lingeringAoeDuration;
        InvokeRepeating(nameof(Damage), 0, 1f); //Deals damage Once every Second
    }

    // Update is called once per frame
    void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            TowerGridPlacement.cellsWithLingeringAoe.Remove(fieldPosition);
            Debug.Log("Deleted Zone");
            Destroy(gameObject);
        }
    }

    void Damage()
    {
        bool didDamage = false;
        if (!towerStats.lingeringAoe)
        {
            foreach (Vector3 pos in EnemyBibleScript.EnemyBible.Keys)
            {
                 Debug.Log(grid.WorldToCell(pos)  + "ist bei" + cellPosition);
                if (grid.WorldToCell(pos) == cellPosition)
                {
                    if (EnemyBibleScript.EnemyBible.ContainsKey(pos))
                    {
                        GameObject nextVictim = EnemyBibleScript.EnemyBible[pos];
                        Health health = nextVictim.GetComponent<Health>();
                        health.health -= towerStats.damage;
                        didDamage = true;
                        Debug.Log("Target found " + nextVictim);
                    }
                }
            }
        }
        else if (towerStats.lingeringAoe)
        {
            foreach (Vector3 pos in EnemyBibleScript.EnemyBible.Keys)
            {
                if (Vector3.Distance(transform.position, pos) < towerStats.aoeSize)
                {
                    if (EnemyBibleScript.EnemyBible.ContainsKey(pos))
                    {
                        GameObject nextVictim = EnemyBibleScript.EnemyBible[pos];
                        Health health = nextVictim.GetComponent<Health>();
                        health.health -= towerStats.lingeringAoeDamage;
                        didDamage = true;
                    }
                }
            }
        }

        if (didDamage)
        {
            //Sound
        }
    }
}
