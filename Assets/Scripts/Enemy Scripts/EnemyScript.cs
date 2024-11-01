using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

//  SCRIPT SUMMARY: enemy brain for movement & attacking

public class EnemyScript : MonoBehaviour
{
    public enum Targets
    {
        Everything,
        MainTower,
        Towers,
        Walls,
        Mines
    }
    [Tooltip("Select which towers this unit will attack. It will try to avoid the others.")]
    public Targets target = Targets.MainTower;
    [Tooltip("all towers within x tiles of the unit")]
    [SerializeField] private int sightRange = 1, attackRange = 1;
    [Tooltip("Damage per attack. Units targeting everything always do base damage")]
    [SerializeField] private float baseDamage, buffedDamage;
    [Tooltip("Time in seconds between attacks")]
    [SerializeField] private float attackCooldown;

    private Grid grid;
    NavMeshAgent agent;
    float t = 1f, cooldown = 0f;
    Vector3 currentPosOnGrid;
    List<GameObject> foundTowers = new List<GameObject>();
    GameObject nextVictim;
    bool canAttackVictim;



    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        EnemyBibleScript.EnemyBible.Add(transform.position, gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        cooldown -= Time.deltaTime;



        // update the agent's destination once every second 
        if (t >= 1)
        {
            t = 0;
            // if unit targets the main tower just try to move forwards while taking the least amount of damage 
            if (target == Targets.MainTower)
            {
                if (agent.enabled == true) agent.SetDestination(transform.position + Vector3.forward * 50f);
            }
            // if unit targets eveything move to the closest tower or keep moving forwards if there are none
            else if (target == Targets.Everything)
            {
                float distance = Mathf.Infinity;

                CheckGridPositions();

                foreach (GameObject tower in foundTowers)
                {
                    TowerKnowsWhereItIs towerScript = tower.GetComponent<TowerKnowsWhereItIs>();
                    foreach (Vector3Int pos in towerScript.MyCells)
                    {
                        if (Vector3.Distance(transform.position, grid.CellToWorld(new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z))) + new Vector3(5, 0, 5)) < distance)
                        {
                            distance = Vector3.Distance(transform.position, grid.CellToWorld(new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z))) + new Vector3(5, 0, 5));
                            nextVictim = tower;
                            if (agent.enabled == true) agent.SetDestination(grid.CellToWorld(new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z))) + new Vector3(5, 0, 5));
                        }
                    }

                }

                if (nextVictim == null)
                {
                    if (agent.enabled == true) agent.SetDestination(transform.position + Vector3.forward * 50f);
                }
            }
            // if unit targets a specific type of tower move to the closest tower of that type or keep moving forwards if there are none
            else
            {
                CheckGridPositions();

                if (foundTowers.Count != 0) SelectTarget();
                if (nextVictim == null)
                {
                    if (agent.enabled == true) agent.SetDestination(transform.position + Vector3.forward * 50f);
                }
            }
        }

        // stop moving if theres a tower in the way
        if (TowerGridPlacement.TowerBible.ContainsKey(grid.WorldToCell(transform.position))) agent.enabled = false;
        else agent.enabled = true;

        // always move to & try to attack the current victim
        if (nextVictim != null)
        {
            if (agent.enabled == true && agent.destination != null) agent.SetDestination(nextVictim.transform.GetChild(0).position);
            CheckAttackRange(nextVictim);
            if (canAttackVictim && cooldown <= 0) Attack();
            if (canAttackVictim && attackRange > 1) 
            {
                agent.enabled = false;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(nextVictim.transform.GetChild(0).position - transform.position), 5 * Time.deltaTime);
            }
        }
       

        // attack towers that aren't a target but are in the way. it's nothing personal :(
        if (!agent.enabled && Vector3.Distance(transform.position, agent.destination) > attackRange * 8)
        {
            float distance = Mathf.Infinity;

            CheckGridPositions();
            foreach (GameObject tower in foundTowers)
            {
                TowerKnowsWhereItIs towerScript = tower.GetComponent<TowerKnowsWhereItIs>();
                foreach (Vector3Int pos in towerScript.MyCells)
                {
                    if (Vector3.Distance(transform.position, grid.CellToWorld(new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z))) + new Vector3(5, 0, 5)) < distance)
                    {
                        distance = Vector3.Distance(transform.position, grid.CellToWorld(new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z))) + new Vector3(5, 0, 5));
                        nextVictim = tower;

                    }
                }

            }
        }

        
    }

    void CheckGridPositions()
    {
        foundTowers.Clear();
        currentPosOnGrid = grid.WorldToCell(transform.position);


        for (int x = -sightRange; x <= sightRange; x++)
        {
            for (int z = -sightRange; z <= sightRange; z++)
            {
                Vector3 offset = new Vector3(x, 0, z);
                Vector3 checkPosition = currentPosOnGrid + offset;
                if (TowerGridPlacement.TowerBible.ContainsKey(checkPosition) && !foundTowers.Contains(TowerGridPlacement.TowerBible[checkPosition]))
                {
                    foundTowers.Add(TowerGridPlacement.TowerBible[checkPosition]);
                }
            }
        }
    }

    void SelectTarget()
    {
        float distance = Mathf.Infinity;
        foreach (GameObject tower in foundTowers)
        {
            if (((target == Targets.Towers && tower.CompareTag("Tower"))
                || (target == Targets.Walls && tower.CompareTag("Wall"))
                || (target == Targets.Mines && tower.CompareTag("Mine"))))
            {
                TowerKnowsWhereItIs towerScript = tower.GetComponent<TowerKnowsWhereItIs>();
                foreach (Vector3Int pos in towerScript.MyCells)
                {
                    if (Vector3.Distance(transform.position, grid.CellToWorld(new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z))) + new Vector3(5, 0, 5)) < distance)
                    {
                        distance = Vector3.Distance(transform.position, grid.CellToWorld(new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z))) + new Vector3(5, 0, 5));
                        nextVictim = tower;
                        if (agent.enabled == true) agent.SetDestination(grid.CellToWorld(new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z))) + new Vector3(5, 0, 5));
                    }
                }

            }
        }
    }

    void CheckAttackRange(GameObject obj)
    {
        float distance = Mathf.Infinity;
        var keys = TowerGridPlacement.TowerBible
            .Where(entry => entry.Value == obj)
            .Select(entry => entry.Key)
            .ToList();
        foreach (var key in keys)
        {
            if (Vector3.Distance(transform.position, grid.CellToWorld(new Vector3Int(Mathf.RoundToInt(key.x), Mathf.RoundToInt(key.y), Mathf.RoundToInt(key.z))) + new Vector3(5, 0, 5)) < distance)
            {

                distance = Vector3.Distance(transform.position, grid.CellToWorld(new Vector3Int(Mathf.RoundToInt(key.x), Mathf.RoundToInt(key.y), Mathf.RoundToInt(key.z))) + new Vector3(5, 0, 5));
            }

        }

        if (distance > attackRange * 8) canAttackVictim = false;
        else canAttackVictim = true;
    }

    void Attack()
    {
        cooldown = attackCooldown;
        Health health = nextVictim.GetComponent<Health>();
        if        ((target == Targets.Towers && nextVictim.CompareTag("Tower"))
                || (target == Targets.Walls && nextVictim.CompareTag("Wall"))
                || (target == Targets.Mines && nextVictim.CompareTag("Mine")))
        {
            health.health -= buffedDamage;
        }
        else
        {
            health.health -= baseDamage;
        }
        
    }
}
