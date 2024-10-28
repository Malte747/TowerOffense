using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

//  SCRIPT SUMMARY: enemy brain for movement, attacking & dying
//      (wip)           create a NavMeshAgent component when the round starts
//      (not started)   calculate easiest path
//      (not started)   choose next target
//      (not started)   attack target
//      (not started)   animations


public class EnemyScript : MonoBehaviour
{
    public Vector3 towerPos;
    public enum Targets
    {
        Main,
        Towers,
        Walls,
        Mines
    }

    [SerializeField] private float sightRange, attackRange;

    public Targets target = Targets.Main;
    NavMeshAgent agent;
    

  
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if(target == Targets.Main)
        {
            agent.SetDestination(towerPos);
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
      if (target == Targets.Towers)
        {

            // walk forward
            // search for towers
            // move to tower 
            // attack tower
        }
    }
}
