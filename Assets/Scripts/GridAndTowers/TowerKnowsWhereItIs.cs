using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerKnowsWhereItIs : MonoBehaviour
{
    [SerializeField] public TowerStats TowerStats;

    public List<Vector3Int> MyCells;


    void Start()

    {
        if (TowerStats.target != TowerStats.Targets.MainTower && gameObject.CompareTag("Tower")) 
        {
            Transform rangeIndicator = gameObject.transform.GetChild(1);
            int rangeNumber = (TowerStats.attackRange * 2);
            rangeIndicator.localScale = new Vector3(rangeNumber, 0.01f, rangeNumber);
        }

    }

}
