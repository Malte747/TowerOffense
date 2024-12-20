using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Mine : MonoBehaviour
{
    private GameManager gameManager;
    public TowerStats towerStats;

    // income animation
    public GameObject incomeAnimationCanvas;
    public TMP_Text incomeText;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.GainIncomeDefender(towerStats.goldProduced);

        StartIncomeAnimation(towerStats.goldProduced);
    }

    // Update is called once per frame
    public void MineIsDying()
    {
        gameManager.GainIncomeDefender(- towerStats.goldProduced);
    }

    private void StartIncomeAnimation(int goldProduced)
    {
        incomeText.text = "+" + goldProduced;
        incomeAnimationCanvas.SetActive(true);
        Destroy(incomeAnimationCanvas, 1f);
    }
}
