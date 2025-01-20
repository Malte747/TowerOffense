using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
        Mines,
        SummonsUnits
    }
    public int cost, supplyCost, income, incomePerSec;
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

    [Header("Necromancer Only")]
    public int minUnits, maxUnits, secondsBetweenSpawns;
    public GameObject[] possibleUnits;
    public float spawnRadius = 10f;
    public float animationSummonDelay = 2f;
    bool spawnedByNecromancer;
  


    private Grid grid;
    NavMeshAgent agent;
    float t = 1f, t2 = 0, cooldown = 0f;
    Vector3 currentPosOnGrid;
    List<GameObject> foundTowers = new List<GameObject>();
    GameObject nextVictim;
    bool canAttackVictim;
    Animator animator;
    private Vector3 lastPosition, destination;
    private Vector3 projectileStartPos;
    private GameObject GameManager;

    // income animation
    public GameObject incomeAnimationCanvas;
    public TMP_Text incomeText;
    public bool dwarf;
    public Animator canvasAnimatorForDwarf;

    // Necromancer
    List<GameObject> SummonUnitList = new List<GameObject>();
    float summonUnitsCooldown = 5;

    // prevent units spawning on top of other units or towers

    private AudioManager unitSFX; // Referenz zum AudioManager Script
    [SerializeField] private int attackSoundNumber1,attackSoundNumber2,spawnSoundNumber1,spawnSoundNumber2;
    private void Awake()
    {
        if(EnemyBibleScript.EnemyBible.ContainsKey(transform.position) || TowerGridPlacement.TowerBible.ContainsKey(transform.position))
        {
            RespectPersonalSpace();
        }
    }

    void RespectPersonalSpace()
    {
        transform.position += new Vector3(0, 0, -1);
        if (EnemyBibleScript.EnemyBible.ContainsKey(transform.position) || TowerGridPlacement.TowerBible.ContainsKey(transform.position))
        {
            RespectPersonalSpace();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("GameManager");
        if(!spawnedByNecromancer) GameManager.GetComponent<GameManager>().GainIncomeAttacker(income);
        unitSFX = GameObject.Find("AudioManager").GetComponent<AudioManager>(); //Nimmt den Audio Manager in das Script
        unitSFX.PlayUnitSound(Random.Range(spawnSoundNumber1,spawnSoundNumber2)); //Spielt die gew�nschte SFX Nummer

        if (target == Targets.SummonsUnits)
        {
            float maxCost = -Mathf.Infinity;
            foreach (GameObject unit in possibleUnits)
            {
                if(unit.GetComponent<EnemyScript>().cost > maxCost)
                {
                    maxCost = unit.GetComponent<EnemyScript>().cost;
                }
            }
            foreach (GameObject unit in possibleUnits)
            {
                for (int i = 0; i < maxCost + 1 - unit.GetComponent<EnemyScript>().cost; i++)
                {
                    SummonUnitList.Add(unit);
                }
            }
        }
        if (incomePerSec != 0)
        {
            GameObject.Find("EnemyPlacementPlane").GetComponent<PlaceEnemies>().combinedIncomePerSec += incomePerSec;
        }
        animator = gameObject.transform.GetChild(1).GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        grid = GameObject.Find("Grid").GetComponent<Grid>();

        if (damageDelay == 0) damageDelay = 0.01f;

        StartCoroutine(StartIncomeAnimationCoroutine());

    }

    // Update is called once per frame
    void Update()
    {
        if(EnemyBibleScript.EnemyBible.ContainsKey(transform.position))
        {
            RespectPersonalSpace();
        }
        EnemyBibleScript.EnemyBible.Add(transform.position, gameObject);
        projectileStartPos = transform.GetChild(0).transform.position;
        t += Time.deltaTime;
        t2 += Time.deltaTime;
        cooldown -= Time.deltaTime;
        if (agent.enabled) destination = agent.destination;


        float distanceToLastFrame = Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;
        if (animator != null) animator.SetFloat("speed", distanceToLastFrame / Time.deltaTime);

        // update the agent's destination once every second 
        if (t >= Random.Range(60, 100) / 100)
        {
            t = 0;
            // if unit targets the main tower just try to move forwards while taking the least amount of damage 
            if (target == Targets.MainTower)
            {
                MoveForwards();
            }
            // Necromancer
            else if (target == Targets.SummonsUnits)
            {
                animator.ResetTrigger("attack");
                if (summonUnitsCooldown <= 0)
                {
                    SummonUnits();
                    summonUnitsCooldown = secondsBetweenSpawns + animationSummonDelay;
                }
                else if (summonUnitsCooldown > secondsBetweenSpawns) 
                {
                    agent.enabled = false;
                    summonUnitsCooldown -= Time.deltaTime;
                }
                else if (summonUnitsCooldown <= animationSummonDelay)
                {
                    agent.enabled = false;
                    summonUnitsCooldown -= Time.deltaTime;
                    animator.SetTrigger("attack");
                }
                else
                {
                    agent.enabled = true;
                    MoveForwards();
                    summonUnitsCooldown -= Time.deltaTime;
                }
                   
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
            TowerGridPlacement.TowerBible.TryGetValue((grid.WorldToCell(transform.position)), out GameObject spikesCheck);
            if (!spikesCheck.CompareTag("LingeringDamage")) agent.enabled = false;
            //Quaternion targetRotation = Quaternion.LookRotation(new Vector3(nextVictim.transform.GetChild(0).position.x - transform.position.x, 0, nextVictim.transform.GetChild(0).position.z - transform.position.z), Vector3.up);
            //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
        }
        else
        {
            agent.enabled = true;
        }

        // attack towers that aren't a target but are in the way. it's nothing personal :(
        if (!agent.enabled && Vector3.Distance(transform.position, agent.destination) > attackRange * 8 && target != Targets.SummonsUnits)
        {
            CheckGridPositions();
            GetClosestinFoundTowers();
        }

        // always move to & try to attack the current victim
        if (nextVictim != null && target != Targets.SummonsUnits)
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




        transform.GetChild(1).localRotation = new Quaternion(0, 0, 0, 1);
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
                    TowerGridPlacement.TowerBible.TryGetValue(checkPosition, out GameObject spikesCheck);
                    if (!spikesCheck.CompareTag("LingeringDamage")) foundTowers.Add(TowerGridPlacement.TowerBible[checkPosition]);
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
        unitSFX.PlayUnitSound(Random.Range(attackSoundNumber1,attackSoundNumber2)); //Spielt die gew�nschte SFX Nummer

        cooldown = attackCooldown;
        if (animator != null)
        {
            animator.SetTrigger("attack");
            transform.GetChild(1).transform.localPosition = Vector3.zero;
        }
        
        else Debug.LogWarning("no animator found");

        if (isRangeUnit) Invoke("Projectile", damageDelay);
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
        HealthTowers health;
        if (nextVictim != null)
        {
            health = nextVictim.GetComponent<HealthTowers>();
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


    private IEnumerator StartIncomeAnimationCoroutine()
    {
        if( !dwarf && !spawnedByNecromancer)
        {
        while (!agent.enabled)
        {
            yield return null;
        }
        if( incomeAnimationCanvas != null)
        {
        incomeText.text = "+" + income;
        incomeAnimationCanvas.SetActive(true);

        yield return new WaitForSeconds(3f);

        Destroy(incomeAnimationCanvas);
        }
        else yield return null;
        }
        else if(dwarf)
        {
        yield return new WaitForSeconds(3f);
        incomeText.text = "+" + incomePerSec;
        incomeAnimationCanvas.SetActive(true);
        while (agent.enabled)
        {
            yield return new WaitForSeconds(3f);
            canvasAnimatorForDwarf.SetTrigger("dwarf");
        }

        }
        
        
    }

    void SummonUnits()
    {
        int random = Random.Range(minUnits, maxUnits);
        for (int i = 0; i <= random; i++)
        {
            // randomize unit
            int random2 = Random.Range(0, SummonUnitList.Count);
            GameObject spawnUnit = SummonUnitList[random2];

            // randomize position
            float randomX = Random.Range(-spawnRadius, spawnRadius);
            float randomZ = Random.Range(-spawnRadius, spawnRadius);
            Vector3 spawnPos = transform.position + new Vector3(randomX, 0, randomZ);

            // spawn unit
            GameObject obj = Instantiate(spawnUnit, spawnPos, Quaternion.identity);
            obj.GetComponent<EnemyScript>().spawnedByNecromancer = true;
        }
    }


}
