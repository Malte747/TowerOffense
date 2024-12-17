using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

//  SCRIPT SUMMARY: enemy brain for movement & attacking
/*
Me when this script compiles:
⣀⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⣿⣿⣿⣶⣤⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠸⣿⣿⣿⠀⠙⠻⣦⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⢹⣿⣿⠀⠀⠀⠈⠙⠷⣦⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⡀
⠀⠀⠹⣿⠀⠀⠀⠀⠀⠀⠈⠙⢷⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⣤⡤⠶⣶⣿⣿⣿⠃
⠀⠀⠀⠹⣯⠀⠀⠀⠀⠀⠀⠀⠀⠙⢷⣄⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣀⣤⡶⠞⠛⠉⠁⠀⠀⣿⣿⣿⠃⠀
⠀⠀⠀⠀⠹⣯⠀⠀⠀⠀⠀⠀⠀⠀⠀⠙⠻⣦⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣀⣤⠶⠞⠋⠉⠀⠀⠀⠀⠀⠀⠀⣸⣿⡿⠃⠀⠀
⠀⠀⠀⠀⠀⠘⢷⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠓⠒⠒⠒⠒⠒⠒⠒⠒⠒⠒⠒⠲⠶⠤⢤⠶⠚⠋⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢰⣿⠟⠁⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠈⢻⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣰⡿⠋⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠹⣦⣀⣴⠟⠂⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡴⠋⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⢨⡿⠓⠂⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣴⡀⠀⠀⣠⡶⠟⠁⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⢀⡾⠁⠀⠀⠀⠀⣀⣄⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠻⣶⠞⠋⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⢀⣾⠁⠀⠀⠀⠀⣼⡏⣹⣿⣧⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⡶⢶⣶⡄⠀⠀⠀⠀⠀⠀⢹⣆⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⣸⠃⠀⠀⠀⠀⠀⢻⣿⣿⣿⠏⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⣷⣾⣿⡿⠀⠀⠀⠀⠀⠀⠀⢻⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⢰⡏⠀⠀⠀⠀⠀⠀⠀⠉⠉⠁⠀⠀⠀⠀⣤⣀⣀⠀⠀⠀⠀⠀⠀⠘⠻⠿⠛⠁⠀⠀⠀⠀⠀⠀⠀⠘⣷⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⣾⢠⣴⣿⣿⣶⣤⠀⠀⠀⠀⠀⠀⠀⠀⠀⠛⠛⠛⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡀⠀⠀⠀⠀⠀⢻⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⡇⣿⣿⣿⣿⣿⣿⠇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣴⣿⣿⣿⣿⣶⡀⠀⠀⠸⣇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⣧⠈⠻⠿⠿⠟⠋⠀⠀⠀⠀⠀⠀⠀⢀⣾⠟⠋⠉⠙⠻⣦⠀⠀⠀⠀⠀⠀⠸⣿⣿⣿⣿⣿⣿⡇⠀⠀⠀⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⢻⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣇⠀⠀⠀⠀⠀  ⣿⠀⠀⠀⠀⠀⠀⠀⠙⠿⠿⠿⠿⠛⠀⠀⠀⠀⢿⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠘⣷⠂⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠙⠷⣤⣀⣠⣴⠟⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠈⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
*/
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
    public int cost, supplyCost, income;
    [Tooltip("Select which towers this unit will attack. It will try to avoid the others.")]
    public Targets target = Targets.MainTower;
    [Tooltip("Unit sees all towers within x tiles")]
    [SerializeField] private int sightRange = 1;
    [Tooltip("Unit can attack all towers within x tiles")]
    [SerializeField] private float attackRange = 1;
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



    private Grid grid;
    NavMeshAgent agent;
    float t = 1f, cooldown = 0f;
    Vector3 currentPosOnGrid;
    List<GameObject> foundTowers = new List<GameObject>();
    GameObject nextVictim;
    bool canAttackVictim;
    Animator animator;
    private Vector3 lastPosition, destination;
    private Vector3 projectileStartPos;
    private GameObject GameManager;


    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("GameManager");
        GameManager.GetComponent<GameManager>().GainIncomeAttacker(income);

        animator = gameObject.transform.GetChild(1).GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        grid = GameObject.Find("Grid").GetComponent<Grid>();

        if(damageDelay == 0) damageDelay = 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyBibleScript.EnemyBible.Add(transform.position, gameObject);
        projectileStartPos = transform.GetChild(0).transform.position;
        t += Time.deltaTime;
        cooldown -= Time.deltaTime;
        if (agent.enabled) destination = agent.destination;


        float distanceToLastFrame = Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;
        if (animator != null) animator.SetFloat("speed", distanceToLastFrame / Time.deltaTime);

        // update the agent's destination once every second 
        if (t >= Random.Range(60,100)/100)
        {
            t = 0;
            // if unit targets the main tower just try to move forwards while taking the least amount of damage 
            if (target == Targets.MainTower)
            {
                MoveForwards();
            }
            // if unit targets eveything move to the closest tower or keep moving forwards if there are none
            else if (target == Targets.Everything)
            {
                CheckGridPositions();
                GetClosestinFoundTowers();
                
                if (nextVictim == null)
                {
                    MoveForwards();
                }
            }
            // if unit targets a specific type of tower move to the closest tower of that type or keep moving forwards if there are none
            else
            {
                CheckGridPositions();

                if (foundTowers.Count != 0) SelectTarget();
                if (nextVictim == null)
                {
                    MoveForwards();
                }
            }
        }

        // stop moving if theres a tower in the way
        if (TowerGridPlacement.TowerBible.ContainsKey(grid.WorldToCell(transform.position)))
        {
            agent.enabled = false;
            //Quaternion targetRotation = Quaternion.LookRotation(new Vector3(nextVictim.transform.GetChild(0).position.x - transform.position.x, 0, nextVictim.transform.GetChild(0).position.z - transform.position.z), Vector3.up);
            //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
        }
        else
        {
            agent.enabled = true;
        }
        

        // always move to & try to attack the current victim
        if (nextVictim != null)
        {
            if (agent.enabled == true && agent.destination != null)
            {
                GetClosestCell(nextVictim);
            }
            CheckAttackRange(nextVictim);
            if (canAttackVictim && cooldown <= 0) Attack();
            if (canAttackVictim && attackRange > 1)
            {
                agent.enabled = false;
                //Quaternion targetRotation = Quaternion.LookRotation( new Vector3(nextVictim.transform.GetChild(0).position.x - transform.position.x, 0, nextVictim.transform.GetChild(0).position.z - transform.position.z), Vector3.up);
                //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
            }
        }


        // attack towers that aren't a target but are in the way. it's nothing personal :(
        if (!agent.enabled && Vector3.Distance(transform.position, agent.destination) > attackRange * 8)
        {
            CheckGridPositions();
            GetClosestinFoundTowers();
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
                || (target == Targets.Mines && tower.CompareTag("Mine")))
                || tower.CompareTag("MainTower"))
            {
                TowerKnowsWhereItIs towerScript = tower.GetComponent<TowerKnowsWhereItIs>();
                foreach (Vector3Int pos in towerScript.MyCells)
                {
                    if (Vector3.Distance(transform.position, grid.CellToWorld(new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z))) + new Vector3(5, 0, 5)) < distance)
                    {
                        distance = Vector3.Distance(transform.position, grid.CellToWorld(new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z))) + new Vector3(5, 0, 5));
                        nextVictim = tower;
                        MoveToGridPos(pos);
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
        if (animator != null) animator.SetTrigger("attack");
        else Debug.LogWarning("no animator found");

        if (isRangeUnit) Projectile();
        else Invoke("Damage", damageDelay);

    }

    void Projectile()
    {
        GameObject yeet = Instantiate(projectile, projectileStartPos, Quaternion.identity);
        Projectile script = yeet.GetComponent<Projectile>();
        script.gotShotBy = gameObject;
        if (nextVictim != null)
        {
            if ((target == Targets.Towers && nextVictim.CompareTag("Tower"))
                    || (target == Targets.Walls && nextVictim.CompareTag("Wall"))
                    || (target == Targets.Mines && nextVictim.CompareTag("Mine")))
            {
                yeet.GetComponent<Projectile>().damage = buffedDamage;
            }
            else
            {
                yeet.GetComponent<Projectile>().damage = baseDamage;
            }
            yeet.GetComponent<Projectile>().victim = nextVictim;
        }

        script.p1 = projectileStartPos;
        script.p3 = destination;

        float height = Vector3.Distance(script.p1, script.p3);
        script.p2 = (projectileStartPos + script.p3) / 2 + Vector3.up * height;
    }

    void Damage()
    {
        Health health;
        if (nextVictim != null)
        {
            health = nextVictim.GetComponent<Health>();
            if ((target == Targets.Towers && nextVictim.CompareTag("Tower"))
                    || (target == Targets.Walls && nextVictim.CompareTag("Wall"))
                    || (target == Targets.Mines && nextVictim.CompareTag("Mine")))
            {
                health.health -= buffedDamage;
                if (nextVictim.CompareTag("MainTower") && !health.attackedMainTower.Contains(gameObject)) health.attackedMainTower.Add(gameObject);
            }
            else
            {
                health.health -= baseDamage;
                if (nextVictim.CompareTag("MainTower") && !health.attackedMainTower.Contains(gameObject)) health.attackedMainTower.Add(gameObject);
            }
        }
        Debug.Log("attack");
    }

    void MoveForwards()
    {
        if (agent.enabled == true) agent.SetDestination(transform.position + Vector3.forward * 10);
    }
    void MoveToGridPos(Vector3 pos)
    {
        if (agent.enabled == true) agent.SetDestination(grid.CellToWorld(new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z))) + new Vector3(5, 0, 5));
    }

    void GetClosestinFoundTowers()
    {
        float distance = Mathf.Infinity;
        Vector3Int closestPos = Vector3Int.zero;
        foreach (GameObject tower in foundTowers)
            {
                TowerKnowsWhereItIs towerScript = tower.GetComponent<TowerKnowsWhereItIs>();
                foreach (Vector3Int pos in towerScript.MyCells)
                {
                    if (Vector3.Distance(transform.position, grid.CellToWorld(new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z))) + new Vector3(5, 0, 5)) < distance)
                    {
                        distance = Vector3.Distance(transform.position, grid.CellToWorld(new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z))) + new Vector3(5, 0, 5));
                        nextVictim = tower;
                        MoveToGridPos(pos);
                    }
                }
            }
    }

    void GetClosestCell(GameObject tower)
    {
        float distance = Mathf.Infinity;
        TowerKnowsWhereItIs towerScript = tower.GetComponent<TowerKnowsWhereItIs>();
        foreach (Vector3Int pos in towerScript.MyCells)
                {
                    if (Vector3.Distance(transform.position, grid.CellToWorld(new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z))) + new Vector3(5, 0, 5)) < distance)
                    {
                        distance = Vector3.Distance(transform.position, grid.CellToWorld(new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z))) + new Vector3(5, 0, 5));
                        nextVictim = tower;
                        MoveToGridPos(pos);
                    }
                }
    }
}
