using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//  SCRIPT SUMMARY: enemy brain for movement, attacking & dying
// get grid position
// check grid positions in FOV
// check for targets
// target found: take ideal path to target 
// target found: DESTROY
// no target found: attack closet tower
// no target found & no towers near: take ideal path forward


public class EnemyScript : MonoBehaviour
{
    public Vector3 towerPos;
    public enum Targets
    {
        None,
        MainTower,
        Towers,
        Walls,
        Mines
    }
    public Targets target = Targets.MainTower;

    [SerializeField] private int sightRange = 1, attackRange = 1;


    private Grid grid;
    NavMeshAgent agent;
    float t = 1f;
    Vector3 currentPosOnGrid;
    List<GameObject> foundTowers = new List<GameObject>();
    GameObject nextVictim;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        EnemyBibleScript.EnemyBible.Add(transform.position, gameObject);


        if (target == Targets.MainTower)
        {
            agent.SetDestination(towerPos);
        }
       

    }

    // Update is called once per frame
    void Update()
    {
         

        t += Time.deltaTime;

        if (target != Targets.MainTower && t >= 1 && nextVictim == null)
        {

            CheckGridPositions();
            t = 0;

            if (foundTowers.Count != 0) SelectTarget();
            else agent.SetDestination(transform.position + Vector3.forward * 100f);
            if (nextVictim == null)
            {
                agent.SetDestination(transform.position + Vector3.forward * 100f);
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
                SearchForTowersAt(checkPosition);
            }
        }
    }

    void SearchForTowersAt(Vector3 pos)
    {
        if (TowerGridPlacement.TowerBible.ContainsKey(pos) && !foundTowers.Contains(TowerGridPlacement.TowerBible[pos]))
        {
            foundTowers.Add(TowerGridPlacement.TowerBible[pos]);
        }
    }

    void SelectTarget()
    {
        List<GameObject> targets = new List<GameObject>();
        float distance = Mathf.Infinity;
        foreach (GameObject tower in foundTowers)
        {
            if (((target == Targets.Towers && tower.CompareTag("Tower"))
                || (target == Targets.Walls && tower.CompareTag("Wall"))
                || (target == Targets.Mines && tower.CompareTag("Mine")))
                && Vector3.Distance(transform.position, tower.transform.position) < distance)
            {
                distance = Vector3.Distance(transform.position, tower.transform.position);
                nextVictim = tower;
                agent.SetDestination(tower.transform.position);
                Debug.Log("Moving to: " + tower.transform.position);
            }
            
            /*
            else if (Vector3.Distance(transform.position, tower.transform.position) < distance)
            {
                distance = Vector3.Distance(transform.position, tower.transform.position);
                nextVictim = tower;
                agent.SetDestination(tower.transform.position);
                Debug.Log("Moving to: " + tower.transform.position);
            }
            */
            
        }
    }
}
