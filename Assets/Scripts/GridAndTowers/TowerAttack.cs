using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    GameObject nextVictim;
    float t = Mathf.Infinity;
    float aoeSize = 5;
    Outline outline;
    private Grid grid;
    Health health;

    public enum Targets
    {
        Closest,
        First,
        LowHP,
        BiggestGroup,
        MainTower
    }
    [Tooltip("Select which units this tower will attack. It will try to avoid the others.")]
    public Targets target = Targets.Closest;
    [Tooltip("Unit sees and can Attack all towers within x tiles")]
    [SerializeField] private int attackRange;
    [Tooltip("Damage per attack. Units targeting everything always do base damage")]
    [SerializeField] private int baseDamage, buffedDamage;
    [Tooltip("Time in seconds between attacks")]
    [SerializeField] private float attackCooldown;
    [Tooltip("Unit does damage x seconds into the attack animation")]
    [SerializeField] private float damageDelay;
    [Tooltip("Does this unit yeet something")]
    [SerializeField] private bool isRangeUnit;
    [Tooltip("If this is a Range Unit assign a projectile here")]
    [SerializeField] private GameObject projectile;
    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        t += Time.deltaTime;
        if (nextVictim != null && IsEnemyInRange(nextVictim.transform.position))
        {
            if (t >= attackCooldown)
            {
                t = 0;
                Damage();
                //Debug.Log("Huuuge Damage");
            }
        }
        else
        {
            SelectTarget();
            //Debug.Log("finding Target");
        }
        
    }

    void SelectTarget()
    {
        float distance = Mathf.Infinity;
        float zPos = -300f;
        float hp = Mathf.Infinity;
        int groupSize = 0;
        foreach (Vector3 pos in EnemyBibleScript.EnemyBible.Keys)
        {
            if (IsEnemyInRange(pos))
            { 
                if (target == Targets.Closest)
                {
                    if (Vector3.Distance(transform.position, pos) <= distance)
                    {
                        VictimFound(pos);
                        distance = Vector3.Distance(transform.position, pos);
                    }
                }
                else if (target == Targets.First)
                {
                    if (pos.z > zPos)
                    {
                        VictimFound(pos);
                        zPos = pos.z;
                    }
                }
                else if (target == Targets.LowHP)
                {
                    Health health;
                    GameObject possibleVictim = EnemyBibleScript.EnemyBible[pos];
                    health = possibleVictim.GetComponent<Health>();
                    if (health.health < hp)
                    {
                        hp = health.health;
                        VictimFound(pos);
                    }
                }
                else if (target == Targets.BiggestGroup)
                {
                    //Debug.Log("group");
                    List<Vector3> group = new List<Vector3>();
                    foreach (Vector3 nearPos in EnemyBibleScript.EnemyBible.Keys)
                    {
                        if (Vector3.Distance(pos, nearPos) <= aoeSize) group.Add(nearPos);
                    }
                    if (group.Count > groupSize)
                    {
                        groupSize = group.Count;
                        VictimFound(pos);
                    }
                }
                else if (target == Targets.MainTower)
                {
                    Vector3 MainTowerPos = new Vector3(0, 0, transform.position.z);
                    if (Vector3.Distance(MainTowerPos, pos) <= distance)
                    {
                        VictimFound(pos);
                        distance = Vector3.Distance(MainTowerPos, pos);
                    }
                }
            }
        }
    }

    void VictimFound(Vector3 pos)
    {
        //Debug.Log("foundVictim");
        nextVictim = EnemyBibleScript.EnemyBible[pos];
        //Mark for Testing
        outline = null;
    }
            
    public bool IsEnemyInRange(Vector3 pos) 
    {
        Vector3Int posOnGrid = grid.WorldToCell(pos);
        Vector3Int towerTile = grid.WorldToCell(transform.position);
        if (posOnGrid != null && posOnGrid.x >= towerTile.x - attackRange && posOnGrid.z >= towerTile.z - attackRange
            && posOnGrid.x <= towerTile.x + attackRange && posOnGrid.z <= towerTile.z + attackRange)
        {
            return true;
        }
        else if (posOnGrid != null && target == Targets.MainTower && posOnGrid.z >= towerTile.z - attackRange
                 &&  posOnGrid.z <= towerTile.z + attackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    void Damage()
    {
        Health health;
        if (nextVictim != null)
        {
            //Mark for Testing
            //if (outline == null) outline = nextVictim.AddComponent<Outline>();

            health = nextVictim.GetComponent<Health>();
            health.health -= baseDamage;
        }
    }
}
