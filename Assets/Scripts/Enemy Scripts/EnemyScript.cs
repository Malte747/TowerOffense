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
    public enum MovementTypes
    {
        Enemy1,
        Enemy2,
        Enemy3
    }

    public MovementTypes MovementType = MovementTypes.Enemy1;
    NavMeshAgent agent;
    public Vector3 towerPos;

  
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(towerPos);
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
