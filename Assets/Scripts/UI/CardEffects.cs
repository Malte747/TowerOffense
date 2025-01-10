using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffects : MonoBehaviour
{
    GameManager gameManager;
    CardSelector cardSelector;
    UIManager uiManager;
    SequentialActivator sequentialActivator;
    

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        cardSelector = GameObject.Find("GameManager").GetComponent<CardSelector>();
        uiManager = GameObject.Find("UiManager").GetComponent<UIManager>();
        sequentialActivator = GameObject.Find("UiManager").GetComponent<SequentialActivator>();
    }

    public void DeleteCards()
    {
        uiManager.TogglePause();
        uiManager.EndCardSelector();
        cardSelector.DeleteCards();
    }

    public void Tier2Upgrade()
    {
        sequentialActivator.BuyTierUpgradeAttack();
        cardSelector.TierUp();
        cardSelector.RemoveCardFromPool("Tier2Upgrade");
         
    }

    public void Tier3Upgrade()
    {
        sequentialActivator.BuyTierUpgradeAttack();
        cardSelector.TierUp();
        cardSelector.RemoveCardFromPool("Tier3Upgrade");
         
    }

    public void SupplyUpgrade(int SupplyIncease)
    {
        gameManager.GainMaxSupplyAttacker(SupplyIncease);
    }

    public void IncomeUpgrade(int IncomeIncease)
    {
        gameManager.GainIncomeAttacker(IncomeIncease);
    }

    public void GoldUpgrade(int GoldIncease)
    {
        gameManager.UnitPayment(-GoldIncease);
    }

    public void FarmerRevolt(int farmers)
    {
        cardSelector.SpawnFarmerPrefabs(farmers);
    }

    public void LowerUpgradePricePercentage(int percentage)
    {
        int percent = 100 - percentage;
        sequentialActivator.LowerUpgradePrice(percent);
    }

    public void LowerUpgradePricePercentageOnce(int percentage)
    {
        int percent = 100 - percentage;
        sequentialActivator.LowerUpgradePrice(percent);
        cardSelector.RemoveCardFromPool("LowerUpgradePriceT3");
    }
    
    




}
