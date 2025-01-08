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


}
