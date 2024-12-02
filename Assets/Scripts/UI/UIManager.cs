using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    GameManager gameManager;
    MessageSystem messageSystem;

    public MultiButtonTrigger multiButtonTriggerAttack;
    public MultiButtonTrigger multiButtonTriggerDefense;


    public GameObject attackerUIBlock;
    public GameObject defenderUIBlock;
    public GameObject attackerSupplyIcon;
    public GameObject defenderSupplyIcon;
    public List<GameObject> gameUI;
    public List<GameObject> additionalGameUI;

    public Button pause;  
    public Button unpause;
    private bool isPaused = false; 
    private bool toggle = true;    

    public Slider hpSlider;
    public TMP_Text hpSliderText;
    public static int towerRepairCost;
    public TMP_Text towerRepairCostText;
    public GameObject[] towerInfoUI;

    

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        messageSystem = GetComponent<MessageSystem>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gameManager.gameInProgress)
        {

            

            if (toggle)
            {
                pause.onClick.Invoke();  
            }
            else
            {
                unpause.onClick.Invoke(); 
            }
            
        }

    }

    public void Unpause()
    {
        if (isPaused)
        {
            Time.timeScale = 1f; // Spiel fortsetzen
            isPaused = false;
            toggle = !toggle;
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            Time.timeScale = 1f; // Spiel fortsetzen
            isPaused = false;
            toggle = !toggle;
        }
        else
        {
            Time.timeScale = 0f; // Spiel pausieren
            isPaused = true;
            toggle = !toggle;
        }
    }


    public void ActivateDefenderUI()
    {
        multiButtonTriggerAttack.ResetMenuNavigation();
        attackerSupplyIcon.SetActive(false);
        defenderSupplyIcon.SetActive(true);
        attackerUIBlock.SetActive(false);
        defenderUIBlock.SetActive(true);


    }

    public void ActivateAttackerUI()
    {
        multiButtonTriggerDefense.ResetMenuNavigation();
        defenderSupplyIcon.SetActive(false);
        attackerSupplyIcon.SetActive(true);
        defenderUIBlock.SetActive(false);
        attackerUIBlock.SetActive(true);
        HideTowerInfoUI();


    }

    public void ResetNavigation()
    {
        multiButtonTriggerAttack.ResetMenuNavigation();
        multiButtonTriggerDefense.ResetMenuNavigation();
        attackerUIBlock.SetActive(false);
        defenderUIBlock.SetActive(true);
    }

    public void StartOrStopGameUI()
    {
         foreach (var obj in gameUI)
        {
            if (obj != null)
            {
                obj.SetActive(!obj.activeSelf); 
            }
        }
    }

    public void StopAdditionalGameUI()
    {
        foreach (var obj in additionalGameUI)
        {
            if (obj != null)
            {
                obj.SetActive(false); 
            }
        }
    }

    #region TowerSelection...Ein Malte und Tom Project

    
    public void SetTowerHPSliderUIValues(int maxHP, int currentHP)
    {

        hpSlider.maxValue = maxHP;

        hpSlider.value = Mathf.Clamp(currentHP, 0, maxHP);

        hpSliderText.text = $"{ hpSlider.value}/{ hpSlider.maxValue}";

        ShowTowerInfoUI();
    }

    public void SetTowerRepairCost(int towerPrice)
    {
        towerRepairCost = towerPrice * gameManager.towerRepairCostMultiplier / 100;
        if(towerRepairCost > 0)
        {
        towerRepairCostText.text = towerRepairCost.ToString();
        }
        else
        {
            towerRepairCostText.text = "FREE";
        }
    }

    public void ShowTowerInfoUI()
    {
        foreach (GameObject obj in towerInfoUI)
        {
            obj.SetActive(true); 
        }
        if (gameManager.attackersTurn)
        {
            towerInfoUI[2].SetActive(false);
            towerInfoUI[3].SetActive(false);
        }
        else if(TowerGridPlacement.clickedTowerParent.GetComponent<Collider>().CompareTag("MainTower"))
        {
            Button button = towerInfoUI[3].GetComponent<Button>();
            button.interactable = false;
            //Debug.Log("MainTower Clicked");
        }
        else
        {
            Button button = towerInfoUI[3].GetComponent<Button>();  
            button.interactable = true;
        }
    }
    public void HideTowerInfoUI()
    {
        foreach (GameObject obj in towerInfoUI)
        {
            obj.SetActive(false); 
        }
    }

    #endregion

    #region MessageSystem

    public void NotEnoughGoldMessage()
    {
        messageSystem.ShowMessage("Nicht genug Gold!"); 
    }

    public void NotEnoughSupplyMessage()
    {
        messageSystem.ShowMessage("Nicht genug Supply!"); 
    }

    public void CannotBuildHereMessage()
    {
        messageSystem.ShowMessage("Hier kannst du nichts bauen!"); 
    }

    public void CannotSpwanUnitsHereMessage()
    {
        messageSystem.ShowMessage("Hier kannst du nichts erschaffen!"); 
    }

    public void UnitsStillFightingMessage()
    {
        messageSystem.ShowMessage("Deine Truppen kämpfen noch!"); 
    }


    #endregion

}
