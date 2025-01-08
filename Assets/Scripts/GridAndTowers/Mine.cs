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
        incomeText = incomeAnimationCanvas.GetComponentInChildren<TMP_Text>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void MineMakesMoney()
    {
        gameManager.GainIncomeDefender(towerStats.goldProduced);
        StartCoroutine(StartIncomeAnimation(towerStats.goldProduced));
    }

    private IEnumerator StartIncomeAnimation(int goldProduced)
    {
        incomeText.text = "+" + goldProduced;
        incomeAnimationCanvas.SetActive(true);
        yield return new WaitForSeconds(1f);
        incomeAnimationCanvas.SetActive(false);
    }
}
