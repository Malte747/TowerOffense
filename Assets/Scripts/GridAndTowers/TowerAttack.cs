using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;
//using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class TowerAttack : MonoBehaviour
{
    GameObject nextVictim;
    float t = Mathf.Infinity;
    float aoeSize = 5;
    Outline outline;
    private Grid grid;

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
    [SerializeField] private int damage;
    [Tooltip("Time in seconds between attacks")]
    [SerializeField] private float attackCooldown;
    [Tooltip("Unit does damage x seconds into the attack animation")]
    [SerializeField] private float damageDelay;
    [Tooltip("Does this unit yeet something")]
    [SerializeField] private bool isRangeUnit;
    [Tooltip("If this is a Range Unit assign a projectile here")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private Vector3 projectileStartPos;

    public enum Animation
    {
        Balista,
        Tower
    }
    [SerializeField] Animation animation = Animation.Tower;

    [SerializeField] ScriptableObject towerAnimationStuff;
    

    private float projectileCorrection;
    private int projectileSpeeed;



    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        projectileStartPos = transform.position + projectileStartPos;

        if (animation == Animation.Balista)
        {

        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        t += Time.deltaTime;
        if (nextVictim != null && IsEnemyInRange(nextVictim.transform.position) && nextVictim.GetComponent<EnemyScript>().enabled)
        {
            if (t >= attackCooldown)
            {
                t = 0;
                if (isRangeUnit)
                {
                    Invoke("ShootProjectile", damageDelay);
                    //Debug.Log("Huuuge Projectile");

                }
                else Invoke("Damage", damageDelay);
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
        float distance = attackRange;
        float zPos = -300f;
        float hp = Mathf.Infinity;
        int groupSize = 0;
        foreach (Vector3 pos in EnemyBibleScript.EnemyBible.Keys)
        {
            if (target == Targets.MainTower)
            {
                break;
            }
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
            }
        }
        if (target == Targets.MainTower)
        {
            //Greift Listeneintrag 0 an
            Health health = GetComponentInParent<Health>();
            if(health.attackedMainTower.Count > 0) VictimFound(health.attackedMainTower[0].transform.position);
        }
    }

    void VictimFound(Vector3 pos)
    {
        //Debug.Log("Found Victim " + Vector3.Distance(pos, transform.position) + " Units away");
        if(EnemyBibleScript.EnemyBible.ContainsKey(pos) ) nextVictim = EnemyBibleScript.EnemyBible[pos];
        //Mark for Testing
        outline = null;
    }
            
    public bool IsEnemyInRange(Vector3 pos) 
    {

        if (Vector3.Distance(transform.position, pos) <= attackRange)
        {
            return true;
        }
        else if (target == Targets.MainTower && Mathf.Abs(transform.position.z - pos.z) <= attackRange)
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
            health.health -= damage;
        }
    }

    void ShootProjectile()
    {
        if (nextVictim != null)
        { 
            StartAnimation();
            GameObject arrow = Instantiate(projectile, projectileStartPos, Quaternion.identity);
            ProjectileTower _projectileTower = arrow.GetComponent<ProjectileTower>();
            _projectileTower.damage = damage;
            _projectileTower.p1 = projectileStartPos;
            _projectileTower.p3 = nextVictim.transform.position;
            float height = (Vector3.Distance(_projectileTower.p1, _projectileTower.p3) / 2);
            _projectileTower.p2 = (projectileStartPos + _projectileTower.p3) / 2 + Vector3.up * height;
            _projectileTower.victim = nextVictim;
            //Debug.Log("Shot Arrow");
        }
    }

    void StartAnimation()
    {
        if (animation == Animation.Balista)
        {
            Animator balistaAnimator = GetComponentInChildren<Animator>();
            balistaAnimator.SetTrigger("ShootBalista");
        }
    }
}
