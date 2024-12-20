using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class TowerAttack : MonoBehaviour
{
    GameObject nextVictim;
    float t = Mathf.Infinity;
    float aoeSize = 5;
    private Grid grid;

    [SerializeField] TowerStats TowerStats;
    
    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        TowerStats.projectileStartPos = transform.position + TowerStats.projectileStartPos;

        if (TowerStats.target != TowerStats.Targets.MainTower)
        {
            Transform rangeIndicator = gameObject.transform.GetChild(1);
            int rangeNumber = (TowerStats.attackRange * 2) + 10;
            rangeIndicator.localScale = new Vector3(rangeNumber, 0.01f, rangeNumber);
        }
            

        if (TowerStats.target == TowerStats.Targets.MainTower)
        {

        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        t += Time.deltaTime;
        if (nextVictim != null && nextVictim.GetComponent<EnemyScript>().enabled && TowerStats.target != TowerStats.Targets.IndicatorTower)
        {
            if (IsEnemyInRange(nextVictim.transform.position) || TowerStats.target == TowerStats.Targets.MainTower) 
            {
                if (t >= TowerStats.attackCooldown)
                {
                    t = 0;
                    if (TowerStats.isRangeUnit)
                    {
                        Invoke("ShootProjectile", TowerStats.damageDelay);
                        //Debug.Log("Huuuge Projectile");

                    }
                    else Invoke("Damage", TowerStats.damageDelay);
                }
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
        float distance = TowerStats.attackRange;
        float zPos = -300f;
        float hp = Mathf.Infinity;
        int groupSize = 0;
        if (TowerStats.target == TowerStats.Targets.MainTower)
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
                    if (TowerStats.target == TowerStats.Targets.Closest)
                    {
                        if (Vector3.Distance(transform.position, pos) <= distance)
                        {
                            VictimFound(pos);
                            distance = Vector3.Distance(transform.position, pos);
                        }
                    }
                    else if (TowerStats.target == TowerStats.Targets.First)
                    {
                        if (pos.z > zPos)
                        {
                            VictimFound(pos);
                            zPos = pos.z;
                        }
                    }
                    else if (TowerStats.target == TowerStats.Targets.LowHP)
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
                    else if (TowerStats.target == TowerStats.Targets.BiggestGroup)
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

        if (Vector3.Distance(transform.position, pos) <= TowerStats.attackRange)
        {
            return true;
        }
        else if (TowerStats.target == TowerStats.Targets.MainTower && Mathf.Abs(transform.position.z - pos.z) <= TowerStats.attackRange)
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
            StartAnimation();
            health = nextVictim.GetComponent<Health>();
            health.health -= TowerStats.damage;
        }
    }

    void ShootProjectile()
    {
        if (nextVictim != null)
        { 
            StartAnimation();
            GameObject arrow = Instantiate(TowerStats.projectile, TowerStats.projectileStartPos, Quaternion.identity);
            ProjectileTower _projectileTower = arrow.GetComponent<ProjectileTower>();
            _projectileTower.TowerStats = TowerStats;
            _projectileTower.p1 = TowerStats.projectileStartPos;
            _projectileTower.p3 = nextVictim.transform.position;
            float height = 0;
            if (TowerStats.projectileCorrection != 0) height = (Vector3.Distance(_projectileTower.p1, _projectileTower.p3) / TowerStats.projectileCorrection);
            _projectileTower.p2 = (TowerStats.projectileStartPos + _projectileTower.p3) / 2 + Vector3.up * height;
            _projectileTower.victim = nextVictim;
            //Debug.Log("Shot Arrow");
        }
    }

    void StartAnimation()
    {
        {
            GameObject _parent = gameObject.transform.root.gameObject;
            Animator[] towerAnimators = _parent.GetComponentsInChildren<Animator>();
            foreach (Animator animator in towerAnimators) animator.SetTrigger(TowerStats.animationTriggerString);   
        }
    }
}
