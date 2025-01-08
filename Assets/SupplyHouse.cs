using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SupplyHouse : MonoBehaviour
{
    private GameManager gameManager;
    public TowerStats towerStats;

    // income animation
    public GameObject supplyAnimationCanvas;
    private TMP_Text supplyText;

    // Start is called before the first frame update
    void Start()
    {
        supplyText = supplyAnimationCanvas.GetComponentInChildren<TMP_Text>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        HouseMakesSupply();
        StartSupplyAnimation(towerStats.supplyProduced);
    }

    public void HouseMakesSupply()
    {
        gameManager.GainMaxSupplyDefender(towerStats.supplyProduced);
        StartSupplyAnimation(towerStats.goldProduced);
    }

    private void StartSupplyAnimation(int goldProduced)
    {
        supplyText.text = "+" + goldProduced;
        supplyAnimationCanvas.SetActive(true);
        Destroy(supplyAnimationCanvas, 1f);
    }
}