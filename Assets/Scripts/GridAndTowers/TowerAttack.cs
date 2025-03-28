using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
//using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class TowerAttack : MonoBehaviour
{
    GameObject nextVictim;
    private float t = Mathf.Infinity;
    private Grid grid;

    [SerializeField] TowerStats towerStats;
    private Vector3 projectileStartPos;
    [SerializeField] private GameObject turningGameObject;
    [SerializeField] private GameObject correctionObject;
    [SerializeField] private Vector3 centerPoint;

    // Start is called before the first frame update
    void Start()
    {
        if (towerStats.towerType != "MainTower") centerPoint = gameObject.transform.GetChild(3).gameObject.transform.position;
        else if (towerStats.towerType == "MainTower") centerPoint = gameObject.transform.position;
            grid = GameObject.Find("Grid").GetComponent<Grid>();
        projectileStartPos = transform.position + towerStats.projectileStartPos;

        if (towerStats.target == TowerStats.Targets.AoeArea)
        {
            AoeDamageZone zoneDamage = gameObject.AddComponent<AoeDamageZone>();
            zoneDamage.towerStats = towerStats;
        }
        else if (towerStats.target == TowerStats.Targets.CellDoesDamage) //Funktioniert so nur für 1x1 tower
        {
            CellDoesDamage cellDamage = gameObject.AddComponent<CellDoesDamage>();
            towerStats.lingeringAoeDamage = towerStats.damage;
            cellDamage.towerStats = towerStats;
        }


    }

    // Update is called once per frame
    void LateUpdate()
    {
        t += Time.deltaTime;
        if (nextVictim != null && nextVictim.GetComponent<EnemyScript>().enabled && towerStats.target != TowerStats.Targets.IndicatorTower && towerStats.target != TowerStats.Targets.AoeArea)
        {
            if (IsEnemyInRange(nextVictim.transform.position) || towerStats.target == TowerStats.Targets.MainTower) 
            {
                if (t >= towerStats.attackCooldown)
                {
                    t = 0;
                    if (towerStats.isRangeUnit)
                    {
                        //Debug.Log("Huuuge Projectile");
                        StartAnimation();
                        Invoke("ShootProjectile", towerStats.damageDelay);
                            
                    }
                    else 
                    {
                        StartAnimation();
                        Invoke("Damage", towerStats.damageDelay); 
                    }
                }
            }
        }
        else
        {
            SelectTarget();
            //Debug.Log("finding Target");
        }    
        
        if (towerStats.needsCorrection)
        {
            if (correctionObject != null) correctionObject.transform.localPosition = towerStats.characterPosition; 
        }
    }

    void SelectTarget()
    {
        float distance = towerStats.attackRange;
        float zPos = -300f;
        float hp = Mathf.Infinity;
        int groupSize = 0;
        if (towerStats.target == TowerStats.Targets.MainTower)
        {
            //Greift Listeneintrag 0 an
            HealthTowers healthTowers = GetComponentInParent<HealthTowers>();
            if (healthTowers.attackedMainTower.Count > 0) VictimFound(healthTowers.attackedMainTower[0].transform.position);
        }
        else
        {
            foreach (Vector3 pos in EnemyBibleScript.EnemyBible.Keys)
            {
                if (IsEnemyInRange(pos))
                {
                    if (towerStats.target == TowerStats.Targets.Closest)
                    {
                        if (Vector3.Distance(centerPoint, pos) <= distance)
                        {
                            VictimFound(pos);
                            distance = Vector3.Distance(centerPoint, pos);
                        }
                    }
                    else if (towerStats.target == TowerStats.Targets.First)
                    {
                        if (pos.z > zPos)
                        {
                            VictimFound(pos);
                            zPos = pos.z;
                        }
                    }
                    else if (towerStats.target == TowerStats.Targets.LowHP)
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
                    else if (towerStats.target == TowerStats.Targets.BiggestGroup)
                    {
                        //Debug.Log("group");
                        List<Vector3> group = new List<Vector3>();
                        foreach (Vector3 nearPos in EnemyBibleScript.EnemyBible.Keys)
                        {
                            if (Vector3.Distance(pos, nearPos) <= towerStats.aoeSize) group.Add(nearPos);
                        }
                        if (group.Count > groupSize)
                        {
                            groupSize = group.Count;
                            VictimFound(pos);
                        }
                    }
                }
            }
        }
    }

    void VictimFound(Vector3 pos)
    {
        //Debug.Log("Found Victim " + Vector3.Distance(pos, transform.position) + " Units away");
        if(EnemyBibleScript.EnemyBible.ContainsKey(pos) ) nextVictim = EnemyBibleScript.EnemyBible[pos];
    }
            
    public bool IsEnemyInRange(Vector3 pos) 
    {

        if (Vector3.Distance(centerPoint, pos) <= towerStats.attackRange)
        {
            return true;
        }
        else if (towerStats.target == TowerStats.Targets.MainTower && Mathf.Abs(transform.position.z - pos.z) <= towerStats.attackRange)
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
        //Debug.Log("Huuuge Damage");
        Health health;
        if (nextVictim != null)
        {
            //Mark for Testing
            //if (outline == null) outline = nextVictim.AddComponent<Outline>();
            health = nextVictim.GetComponent<Health>();
            health.health -= towerStats.damage;
        }
    }

    void ShootProjectile()
    {
        if (nextVictim != null)
        {
            Vector3 directionToTarget = nextVictim.transform.position - centerPoint;
            Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);
            GameObject arrow = Instantiate(towerStats.projectile, projectileStartPos, rotationToTarget);
            ProjectileTower _projectileTower = arrow.GetComponent<ProjectileTower>();
            _projectileTower.TowerStats = towerStats;
            _projectileTower.p1 = projectileStartPos;
            _projectileTower.p3 = nextVictim.transform.position;
            float height = 0;
            if (towerStats.projectileCorrection != 0) height = (Vector3.Distance(_projectileTower.p1, _projectileTower.p3) / towerStats.projectileCorrection);
            _projectileTower.p2 = (projectileStartPos + _projectileTower.p3) / 2 + Vector3.up * height;
            _projectileTower.victim = nextVictim;
            //Debug.Log("Shot Arrow");
        }
    }

    void StartAnimation()
    {
        {
            if (turningGameObject != null)
            {
                Vector3 targetPosition = new Vector3(nextVictim.transform.position.x, turningGameObject.transform.position.y, nextVictim.transform.position.z);
                turningGameObject.transform.LookAt(targetPosition);
            }
            GameObject _parent = gameObject.transform.root.gameObject;
            Animator[] towerAnimators = _parent.GetComponentsInChildren<Animator>();
            foreach (Animator animator in towerAnimators) animator.SetTrigger(towerStats.animationTriggerString);   
        }
    }
}
