using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    GameManager gameManager;
    MessageSystem messageSystem;
     AudioManager audioManager;

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
    public bool isPaused = false; 
    private bool toggle = true;    

    // public Animator animatorGold;
    public Animator[] animatorsToChange;
    public Slider hpSlider;
    public TMP_Text hpSliderText;
    public static int towerRepairCost, combinedRepairCost;
    public TMP_Text towerRepairCostText, towerRepairAllCostText;
    public Image towerImage;
    public GameObject[] towerInfoUI;



    // Time Manager

    private float[] timeScales = { 0.5f, 1f, 2f, 5f, 10f };
    private int currentTimeScaleIndex = 1;
    public bool timePaused;
    public TMP_Text currentTimeScaleIndexText;
    public GameObject pauseIcon;
    public GameObject resumeIcon;

    //Menu UI

    public GameObject resumeGame;
    public GameObject cardSelector;
    public bool cardSelectionInProgress;
    public GameObject tRexButton;
    public Button tRexButtonInteract;

    //How To Play

    public Button PlayButton;
    public Toggle hTPToggle;
    public bool HowToPlayWantsToBeSeen = false;
    public GameObject hTPScreen;
    private bool hTPOpen = false;
    

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        messageSystem = GetComponent<MessageSystem>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        hTPToggle.onValueChanged.AddListener(delegate { ToggleChanged(); });

        ActivateResumeGameButton();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gameManager.gameInProgress && !cardSelectionInProgress && !hTPOpen)
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
            foreach (Animator animator in animatorsToChange)
            {
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            }
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            if(!timePaused)
            {
            Time.timeScale = 1f; // Spiel fortsetzen
            }
            isPaused = false;
            toggle = !toggle;
            
            foreach (Animator animator in animatorsToChange)
            {
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            }
        }
        else
        {
            Time.timeScale = 0f; // Spiel pausieren
            isPaused = true;
            toggle = !toggle;

            foreach (Animator animator in animatorsToChange)
            {
            animator.updateMode = AnimatorUpdateMode.Normal;
            }
        }
    }


    #region Time Management


    public void PauseTime()
    {
        if (!timePaused)
        {
        Time.timeScale = 0f;
        timePaused = true;
        pauseIcon.SetActive(false);
        resumeIcon.SetActive(true);
        }
        else
        {
        Time.timeScale = timeScales[currentTimeScaleIndex];
        timePaused = false;
        pauseIcon.SetActive(true);
        resumeIcon.SetActive(false);
        }
    }

    public void SpeedUpTime()
    {
        if (currentTimeScaleIndex < timeScales.Length - 1)
        {
            currentTimeScaleIndex++;
            currentTimeScaleIndexText.text = timeScales[currentTimeScaleIndex].ToString() + "x";
            if(!timePaused)
            {
            Time.timeScale = timeScales[currentTimeScaleIndex];
            }
           
        }
    }

    public void SlowDownTime()
    {
        if (currentTimeScaleIndex > 0)
        {
            currentTimeScaleIndex--;
            currentTimeScaleIndexText.text = timeScales[currentTimeScaleIndex].ToString() + "x";
                        if(!timePaused)
            {
            Time.timeScale = timeScales[currentTimeScaleIndex];
            }
        }
    }

    public void ResetTimeScale()
    {
            currentTimeScaleIndex = 1;
            Time.timeScale = timeScales[currentTimeScaleIndex];
            currentTimeScaleIndexText.text = timeScales[currentTimeScaleIndex].ToString() + "x";
            if(timePaused)
            {
                PauseTime();
            }
    }

    public void ResetTimeScaleResumeAttacker()
    {
            currentTimeScaleIndex = 1;
            Time.timeScale = timeScales[currentTimeScaleIndex];
            currentTimeScaleIndexText.text = timeScales[currentTimeScaleIndex].ToString() + "x";
            Time.timeScale = 0f;
            timePaused = true;
            pauseIcon.SetActive(false);
            resumeIcon.SetActive(true);
    }









    #endregion


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
        DisableTRex();
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

    public void ActivateResumeGameButton()
    {
        if(gameManager.gameInMemory == true)
        {
        resumeGame.SetActive(true);
        }
        else
        {
        resumeGame.SetActive(false);
        }
    }


    public void CloseTheGame()
    {
        Application.Quit();
    }

    #region TowerSelection...Ein Malte und Tom Project

    
    public void SetTowerHPSliderUIValues(int maxHP, int currentHP)
    {

        hpSlider.maxValue = maxHP;

        hpSlider.value = Mathf.Clamp(currentHP, 0, maxHP);

        hpSliderText.text = $"{ hpSlider.value}/{ hpSlider.maxValue}";
    }

    public void SetTowerRepairCost(int towerPrice, int maxHP, int currentHP)
    {
        
        float towerRepairCostFloat = ((float)towerPrice / maxHP) * ((float)gameManager.towerRepairCostMultiplier / 100f) * (maxHP - currentHP);
        towerRepairCost = Mathf.RoundToInt(towerRepairCostFloat);
        Debug.Log("reapir cost:" + towerRepairCost);
        if(towerRepairCost >= 1)
        {
        towerRepairCostText.text = towerRepairCost.ToString();
        }
        else if(towerRepairCost < 1 && towerRepairCost > 0)
        {
            towerRepairCost = 1;
            towerRepairCostText.text = towerRepairCost.ToString();
        }
        else
        {
            towerRepairCostText.text = "FREE";
        }
    }

    public void SetTowerAllRepairCost()
    {
        List<GameObject> taggedObjects = new List<GameObject>();
        combinedRepairCost = 0;
        foreach (GameObject obj in TowerGridPlacement.TowerBible.Values)
        {
            if (obj != null && !taggedObjects.Contains(obj))
            {
                taggedObjects.Add(obj);

                HealthTowers health = obj.GetComponent<HealthTowers>();
                TowerStats stats = health.TowerStats;
                int towerPrice = stats.goldCost;
                int maxHP = stats.health;
                int currentHP = health.health;
                float towerRepairCostFloat = ((float)towerPrice / maxHP) * ((float)gameManager.towerRepairCostMultiplier / 100f) * (maxHP - currentHP);
                combinedRepairCost += Mathf.RoundToInt(towerRepairCostFloat);
            }
        }   
        Debug.Log("Combined reapair cost: " + combinedRepairCost);
        if (combinedRepairCost >= 1)
        {
            towerRepairAllCostText.text = combinedRepairCost.ToString();
        }
        else if (combinedRepairCost < 1 && combinedRepairCost > 0)
        {
            combinedRepairCost = 1;
            towerRepairAllCostText.text = combinedRepairCost.ToString();
        }
        else
        {
            towerRepairAllCostText.text = "FREE";
        }
    }


    public void SetTowerImage(Sprite image)
    {
        towerImage.sprite = image;
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
        audioManager.PlayUISound(22);
    }

    public void NotEnoughSupplyMessage()
    {
        messageSystem.ShowMessage("Nicht genug Supply!");
         audioManager.PlayUISound(22); 
    }

    public void CannotBuildHereMessage()
    {
        messageSystem.ShowMessage("Hier kannst du nichts bauen!"); 
        audioManager.PlayUISound(22);
    }

    public void CannotSpwanUnitsHereMessage()
    {
        messageSystem.ShowMessage("Hier kannst du nichts erschaffen!");
        audioManager.PlayUISound(22);
    }

    public void UnitsStillFightingMessage()
    {
        messageSystem.ShowMessage("Unsere Truppen sind im Kampf!");
        audioManager.PlayUISound(22); 
    }
    public void TimePausedMessage()
    {
        messageSystem.ShowMessage("Die Zeit ist noch angehalten!");
        audioManager.PlayUISound(22); 
    }


    #endregion

    #region CardSelector


    public void StartCardSelector()
    {
        cardSelector.SetActive(true);
        cardSelectionInProgress = true;
    }
    public void EndCardSelector()
    {
        cardSelector.SetActive(false);
        cardSelectionInProgress = false;
    }

    public void EnableTRex()
    {
        
        tRexButtonInteract.interactable = true;
        tRexButton.SetActive(true);
        tRexButton.tag = "TierButtonActive";

    }

    public void DisableTRex()
    {
        tRexButtonInteract.interactable = false;
        tRexButton.SetActive(false);
        tRexButton.tag = "TierButtonInactive";
    }




    #endregion

    #region HowToPlay

    public void CheckIfHowToPlayWantsToBeSeen()
    {
        if(HowToPlayWantsToBeSeen == true)
        {
            PlayButton.onClick.Invoke();
        }
        else
        {
            hTPScreen.SetActive(true);
        }
    }

    public void ToggleChanged()
    {
        // Ändere den Bool basierend auf dem Toggle-Zustand
        UpdateBool(hTPToggle.isOn);
    }

    private void UpdateBool(bool isOn)
    {
        HowToPlayWantsToBeSeen = isOn;
        Debug.Log("myBool ist jetzt: " + HowToPlayWantsToBeSeen);
    }

    public void OpenhTP()
    {
        if(hTPOpen)
        {
            hTPOpen = false;
        }
        else 
        {
            hTPOpen = true;
        }
    }

    #endregion
}
