using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static EnemyScript;
using static UnityEditor.PlayerSettings;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.GraphicsBuffer;

public class TowerAttack : MonoBehaviour
{
    GameObject nextVictim;
    float t = Mathf.Infinity;
    float aoeSize = 1;

    public enum Targets
    {
        Closest,
        First,
        LowHP,
        BiggestGroup
    }
    [Tooltip("Select which units this tower will attack. It will try to avoid the others.")]
    public Targets target = Targets.Closest;
    [Tooltip("Unit sees and can Attack all towers within x tiles")]
    [SerializeField] private int attackRange;
    [Tooltip("Damage per attack. Units targeting everything always do base damage")]
    [SerializeField] private float baseDamage, buffedDamage;
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
        attackRange = attackRange * 10;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        t += Time.deltaTime;
        if (t >= attackCooldown)
        {
            t = 0;
            if (nextVictim != null && Vector3.Distance(transform.position, nextVictim.transform.position) <= attackRange)
            {
                Damage();
                //Debug.Log("Huuuge Damage");
            }
            else
            {
                SelectTarget();
            }
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
                List<Vector3> group = new List<Vector3>();
                foreach (Vector3 nearPos in EnemyBibleScript.EnemyBible.Keys)
                {
                    if(Vector3.Distance(pos, nearPos) <= aoeSize) group.Add(nearPos);
                }
                if(group.Count > groupSize) 
                { 
                    groupSize = group.Count;
                    VictimFound(pos);
                }
            }
        }
    }

    void VictimFound(Vector3 pos)
    {
        Debug.Log("foundVictim");
        nextVictim = EnemyBibleScript.EnemyBible[pos];
    }


    void Damage()
    {
        Health health;
        if (nextVictim != null)
        {
            //Mark for Testing
            Outline outline = nextVictim.AddComponent<Outline>();
            if (outline != null) outline.enabled = true;
        
            health = nextVictim.GetComponent<Health>();
            /*if ((target == Targets.Towers && nextVictim.CompareTag("Tower"))
            || (target == Targets.Walls && nextVictim.CompareTag("Wall"))
                    || (target == Targets.Mines && nextVictim.CompareTag("Mine")))
            {
                health.health -= buffedDamage;
            }
            else
            {
            */
                health.health -= baseDamage;
            //}
        }
    }
}
