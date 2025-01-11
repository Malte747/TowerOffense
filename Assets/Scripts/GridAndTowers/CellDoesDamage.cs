using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellDoesDamage : MonoBehaviour
{
    private Grid grid;
    public Vector3Int cellPosition;
    public float duration;
    public TowerStats towerStats;

    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.Find("Grid").GetComponent<Grid>();

        if (TowerGridPlacement.cellsWithLingeringAoe.ContainsKey(cellPosition) && towerStats.target != TowerStats.Targets.CellDoesDamage) //If there would be 2 lingeringAoe on the same cell the one with lower damage gets deleted
        {
            Debug.Log("a");
            TowerGridPlacement.cellsWithLingeringAoe.TryGetValue(cellPosition, out GameObject otherAoe);
            TowerStats otherAoeStats = otherAoe.GetComponent<HealthTowers>().TowerStats;
            if (otherAoe.GetComponent<CellDoesDamage>().duration * otherAoeStats.lingeringAoeDamage > duration * towerStats.lingeringAoeDamage) Destroy(gameObject);
            else Destroy(otherAoe);
        }
        else if (towerStats.target != TowerStats.Targets.CellDoesDamage) TowerGridPlacement.cellsWithLingeringAoe.Add(cellPosition, gameObject);
        if (towerStats.target == TowerStats.Targets.CellDoesDamage) duration = Mathf.Infinity;
        else duration = towerStats.lingeringAoeDuration;
        InvokeRepeating(nameof(Damage), 0, 0.5f); //Deals damage Once every Second
    }

    // Update is called once per frame
    void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            TowerGridPlacement.cellsWithLingeringAoe.Remove(cellPosition);
            Debug.Log("Deleted Zone");
            Destroy(gameObject);
        }
    }

    void Damage()
    {
        bool didDamage = false;
        foreach (Vector3 pos in EnemyBibleScript.EnemyBible.Keys)
        {
            if (grid.WorldToCell(pos) == cellPosition)
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
        if (didDamage)
        {
            //Sound
        }
    }
}
