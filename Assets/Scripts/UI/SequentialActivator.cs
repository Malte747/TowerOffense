using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class SequentialActivator : MonoBehaviour
{
    GameManager gameManager;

    public List<GameObject> objectsToActivateDefense; // Liste der GameObjects
    public List<GameObject> objectsToActivateTierDefense;
    public List<GameObject> objectsToActivateAttack;
    public List<GameObject> objectsToActivateTierAttack;
    private int currentIndex = 0; // Der aktuell aktivierte Index
    private int currentIndexAttack = 0;

    public int Tier2Price = 10;
    public int Tier3Price = 100;

    //Text
    [Header("text")]
    
    [SerializeField] public TMP_Text tierTextAttack;
    [SerializeField] public TMP_Text goldPriceTextAttack;
    [SerializeField] public TMP_Text tierTextDefense;    
    [SerializeField] public TMP_Text goldPriceTextDefense;


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        tierTextAttack.text = "Upgraden zu Tier 2";
        tierTextDefense.text = "Upgraden zu Tier 2";
        goldPriceTextAttack.text = Tier2Price.ToString();
        goldPriceTextDefense.text = Tier2Price.ToString();

    }


    public void BuyTierUpgradeDefense()
    {
        if(currentIndex + 2 <= objectsToActivateDefense.Count)
        {
            if(currentIndex == 0 && GameManager.defenderGold >= Tier2Price)
            {
                gameManager.TurretPayment(Tier2Price);
                ActivateNextObject();
                tierTextDefense.text = "Upgraden zu Tier 3";
                goldPriceTextDefense.text = Tier3Price.ToString();
            }
            else if(currentIndex == 1 && GameManager.defenderGold >= Tier3Price)
            {
                gameManager.TurretPayment(Tier3Price);
                ActivateNextObject();
                tierTextDefense.text = "Max Tier";
                goldPriceTextDefense.text = "-";
            }
            else
            {
                Debug.Log("Not enough Gold");
            }
        }
    }

    public void BuyTierUpgradeAttack()
    {
        if(currentIndexAttack + 2 <= objectsToActivateAttack.Count)
        {
             if(currentIndexAttack == 0 && GameManager.attackerGold >= Tier2Price)
            {
                gameManager.UnitPayment(Tier2Price);
                ActivateNextObjectAttack();
                tierTextAttack.text = "Upgraden zu Tier 3";
                goldPriceTextAttack.text = Tier3Price.ToString();

            }
            else if(currentIndexAttack == 1 && GameManager.attackerGold  >= Tier3Price)
            {
                gameManager.UnitPayment(Tier3Price);
                ActivateNextObjectAttack();
                tierTextAttack.text = "Max Tier";
                goldPriceTextAttack.text = "-";
            }
            else
            {
                Debug.Log("Not enough Gold");
            }
        }
    }


    public void ActivateNextObject()
    {

        foreach (var obj in objectsToActivateDefense)
        {
            obj.SetActive(false);
        }
        foreach (var obj in objectsToActivateTierDefense)
        {
            SetButtonsInteractable(obj, false);
        }


        currentIndex = (currentIndex + 1) % objectsToActivateDefense.Count;


        objectsToActivateDefense[currentIndex].SetActive(true);


        for (int i = 0; i <= currentIndex; i++)
        {
            SetButtonsInteractable(objectsToActivateTierDefense[i], true);
        }
    }

        public void ActivateNextObjectAttack()
    {
        // Deaktiviert alle Objekte in der Liste
        foreach (var obj in objectsToActivateAttack)
        {
            obj.SetActive(false);
        }
        foreach (var obj in objectsToActivateTierAttack)
        {
            SetButtonsInteractable(obj, false);
        }

        // Inkrementiere den Index, und wenn das Ende der Liste erreicht ist, starte von vorn
        currentIndexAttack = (currentIndexAttack + 1) % objectsToActivateAttack.Count;

        // Aktiviere das nächste Objekt in der Liste
        objectsToActivateAttack[currentIndexAttack].SetActive(true);

            for (int i = 0; i <= currentIndexAttack; i++)
        {
            SetButtonsInteractable(objectsToActivateTierAttack[i], true);
        }
    }

private void SetButtonsInteractable(GameObject obj, bool interactable)
{
    Button[] buttons = obj.GetComponentsInChildren<Button>(true);
    foreach (Button button in buttons)
    {
        button.interactable = interactable;

        // Setze den Tag basierend auf dem Zustand des Buttons
        if (interactable)
        {
            button.tag = "TierButtonActive"; // Tag ändern, wenn der Button aktiviert ist
        }
        else
        {
            button.tag = "TierButtonInactive"; // Tag zurücksetzen, wenn der Button deaktiviert ist
        }
    }
}



public void ResetUITierButtons()
{
    // Setze nur die Buttons der Objekte mit Index 1 und 2 in Tier-Listen auf "TierButtonInactive" und deaktiviere sie
    if (objectsToActivateTierDefense.Count > 1)
    {
        SetButtonsInteractable(objectsToActivateTierDefense[1], false);
    }
    if (objectsToActivateTierDefense.Count > 2)
    {
        SetButtonsInteractable(objectsToActivateTierDefense[2], false);
    }

    if (objectsToActivateTierAttack.Count > 1)
    {
        SetButtonsInteractable(objectsToActivateTierAttack[1], false);
    }
    if (objectsToActivateTierAttack.Count > 2)
    {
        SetButtonsInteractable(objectsToActivateTierAttack[2], false);
    }

    if (objectsToActivateDefense.Count > 1)
    {
        objectsToActivateDefense[1].SetActive(false);
    }
    if (objectsToActivateDefense.Count > 2)
    {
        objectsToActivateDefense[2].SetActive(false);
    }

    if (objectsToActivateAttack.Count > 1)
    {
        objectsToActivateAttack[1].SetActive(false);
    }
    if (objectsToActivateAttack.Count > 2)
    {
        objectsToActivateAttack[2].SetActive(false);
    }

    // Aktiviere das Element an Index 0 in Defense- und Attack-Listen
    if (objectsToActivateDefense.Count > 0)
    {
        objectsToActivateDefense[0].SetActive(true);
    }
    if (objectsToActivateAttack.Count > 0)
    {
        objectsToActivateAttack[0].SetActive(true);
    }
    
    // Zurücksetzen der Texte
    tierTextAttack.text = "Upgraden zu Tier 2";
    tierTextDefense.text = "Upgraden zu Tier 2";
    goldPriceTextAttack.text = Tier2Price.ToString();
    goldPriceTextDefense.text = Tier2Price.ToString();

    // Zurücksetzen der Indizes
    currentIndex = 0;
    currentIndexAttack = 0;
}
}

