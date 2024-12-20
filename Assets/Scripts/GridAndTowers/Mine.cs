using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    private GameManager gameManager;
    public TowerStats towerStats;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.GainIncomeDefender(towerStats.goldProduced);
    }

    // Update is called once per frame
    public void MineIsDying()
    {
        gameManager.GainIncomeDefender(- towerStats.goldProduced);
    }
}
