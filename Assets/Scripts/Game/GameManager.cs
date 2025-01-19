using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    UIManager uiManager;
    PlaceEnemies placeEnemies;
    TowerGridPlacement towerGridPlacement;
    SequentialActivator sequentialActivator;
    AudioManager audioManager;
    CameraController cameraController;
    CardSelector cardSelector;



    //Turns
    [Header("Turns")]

    [SerializeField] public bool attackersTurn = false;
    [SerializeField] public bool defendersTurn = true;
    [SerializeField] public int currentTurn = 1;
    public int maxTurnCount;



    //Attackers stats
    [Header("Attacker Stats")]
    public int attackerStartGold;
    [SerializeField] private int attackerGoldIncome;
    public int attackerMaxSupply;
    



    //defenders stats
    [Header("Defender Stats")]
    public int defenderStartGold;
    [SerializeField] private int defenderGoldIncome;
    public int defenderMaxSupply;
    


    //Allgemeine stats
    [Header("Allgemeine Stats")]

    
    public int startGoldIncome;
    public bool gameInProgress = false;
    public bool gameInMemory = false;
    public int towerRepairCostMultiplier;


    //static values
    public int attackerGold;
    public int defenderGold; 
    public int attackerSupply;
    public int defenderSupply;
    public int maxSupply = 20;

    //Game Victory

    [Header("Victory Screen")]
    public GameObject victoryScreen;
    public GameObject imageDefender;
    public GameObject imageAttacker;
    public GameObject winnerTextAttacker;
    public GameObject winnerTextDefender;
    bool resettingUnits = false;



    // Text
    [Header("Texts")]

    [SerializeField] public TMP_Text goldText;
    [SerializeField] public TMP_Text currentIncomeText;
    [SerializeField] public TMP_Text supplyText;
    [SerializeField] public TMP_Text maxSupplyText;
    [SerializeField] public TMP_Text currentTurnText;
    [SerializeField] public TMP_Text maxTurnCountText;
    [SerializeField] public TMP_Text roundTextDefense;
    [SerializeField] public TMP_Text roundTextAttack;
    private int goldValue; 
    private int currentIncomeValue; 
    private int supplyValue;
    private int maxSupplyValue;
    private int currentTurnValue;
    private int maxTurnCountValue;

    //Text Animation

    [Header("Text Animation")]

    public GameObject roundTextDefenseObject;
    public GameObject roundTextAttackObject;
    public Animator goldTextAnimator;
    public Animator goldIncomeTextAnimator;
    public Animator attackerRoundTextAnimator;
    public Animator defenderRoundTextAnimator;

    private float animationDuration = 1.0f;
    private int lastSpending;
    private int latestSpending;
    private Queue<int> updateQueue = new Queue<int>();
    private Queue<int> spendingQueue = new Queue<int>();
    private bool isUpdating = false; 

    private int incomeBonus;
    private int latestIncomeSpending;
    private int incomeValue;
    private Queue<int> updateIncomeQueue = new Queue<int>();
    private Queue<int> spendingIncomeQueue = new Queue<int>();
    private bool isUpdatingIncome = false; 
  

  // UI Animation

      public Animator UICanvasAnimator;

    public Animator PauseButtonAnimator;



    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        sequentialActivator = GameObject.Find("UiManager").GetComponent<SequentialActivator>();
        towerGridPlacement = GameObject.Find("TowerGridPlacement").GetComponent<TowerGridPlacement>();
        placeEnemies = GameObject.Find("EnemyPlacementPlane").GetComponent<PlaceEnemies>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        cameraController = GameObject.Find("Camera Rig").GetComponent<CameraController>();
        cardSelector = GameObject.Find("GameManager").GetComponent<CardSelector>();
    }

    public void GameStart()
    {
        attackerGold = attackerStartGold;
        attackerGoldIncome = startGoldIncome;
        

        defenderGold = defenderStartGold;
        defenderGoldIncome = startGoldIncome;

        attackerMaxSupply = maxSupply;
        defenderMaxSupply = maxSupply;

        SetMaxSupplyValue(defenderMaxSupply);
        SetCurrentTurnValue(currentTurn);
        SetMaxTurnCountValue(maxTurnCount);
        

        gameInProgress = true;
        gameInMemory = true;

        DefendersTurn();
        StartRoundUIAnimation();
    }



    private void AttackersTurn()
    {
        if (attackersTurn)
        {
            attackerGold = attackerGold + attackerGoldIncome;
            attackerSupply = 0;
            goldTextAnimator.SetTrigger("GoldAnimation");
            goldIncomeTextAnimator.SetTrigger("IncomeAnimation");
            StartCoroutine(AnimateGoldTextRoundStartAttack());
            SetIncomeText(attackerGoldIncome);
            SetSupplyValue(attackerSupply);
            SetMaxSupplyValue(attackerMaxSupply);
            roundTextAttackObject.SetActive(true);
            uiManager.ResetTimeScale();
            cameraController.MoveCameraToAttackerPosition();
            audioManager.PlayUISound(13); 
            
        }
    }

    private void DefendersTurn()
    {
        if (defendersTurn)
        {
            defenderGold = defenderGold + defenderGoldIncome;
            
            StartCoroutine(AnimateGoldTextRoundStartDefense());
            goldTextAnimator.SetTrigger("GoldAnimation");
            goldIncomeTextAnimator.SetTrigger("IncomeAnimation");
            SetSupplyValue(defenderSupply);
            SetIncomeText(defenderGoldIncome);
            SetMaxSupplyValue(defenderMaxSupply);
            roundTextDefenseObject.SetActive(true);
            uiManager.ResetTimeScale();
            cameraController.MoveCameraToDefenderPosition();
            audioManager.PlayUISound(12); 
            Invoke("GenerateMineIncome", 3f);
            
           
            
        }
    }

    public void EndTrun()
    {
        if(uiManager.timePaused)
        {
            uiManager.TimePausedMessage();
        }
        else if(maxTurnCount == currentTurn && attackersTurn)
        {
            EndGameDefenderWin();
        }
        else if (attackersTurn && EnemyBibleScript.EnemyBible.Count == 0)
        {
            attackersTurn = false;
            defendersTurn = true;
            currentTurn++;
            SetCurrentTurnValue(currentTurn);
            uiManager.ActivateDefenderUI();
            roundTextAttackObject.SetActive(false);
            DefendersTurn();
            audioManager.ChangeTrackOnRound();
            ChangeRoundUIAnimation();
        }
        else if (defendersTurn)
        {
            defendersTurn = false;
            attackersTurn = true;
            uiManager.ActivateAttackerUI();
            roundTextDefenseObject.SetActive(false);
            AttackersTurn();
            audioManager.ChangeTrackOnRound();
            uiManager.PauseTime();
            ChangeRoundUIAnimation();
            PauseButtonAnimationBoolTrue();
        }
        else
        {
            uiManager.UnitsStillFightingMessage();
        }
    }

    public void CheckIfTurnIsOver()
    {
        if (EnemyBibleScript.EnemyBible.Count == 0 && attackerGold < 5)
        {
            StartCoroutine(TurnIsOver());
        }
    }

    private IEnumerator TurnIsOver()
    {
        yield return new WaitForSecondsRealtime(3f);

        if (attackersTurn && EnemyBibleScript.EnemyBible.Count == 0)
        {
            EndTrun();
        }
    }


    #region GoldAnimation
    public void EnqueueGoldUpdate(int newGoldValue, int spending)
    {
        
        updateQueue.Enqueue(newGoldValue);
        spendingQueue.Enqueue(spending);
        

       
        if (!isUpdating)
        {
            StartCoroutine(ProcessQueue());
        }
    }

    private IEnumerator ProcessQueue()
    {
        isUpdating = true;

        while (updateQueue.Count > 0)
        {
           
            goldValue = updateQueue.Dequeue();
            latestSpending = spendingQueue.Dequeue();
            yield return StartCoroutine(UpdateGoldText());
        }

        isUpdating = false; 
    }

private IEnumerator UpdateGoldText()
{
    int startValue = goldValue + latestSpending;

    if (latestSpending <= 10)
    {
        animationDuration = 0.1f;
    }
    else if (latestSpending <= 50)
    {
        animationDuration = 0.2f;
    }
    else if (latestSpending <= 100)
    {
        animationDuration = 0.3f;
    }
    else
    {
        animationDuration = 1f;
    }

    float elapsedTime = 0f;
    float rate = 2.0f;

    while (elapsedTime < animationDuration)
    {
        float progress = Mathf.Pow(rate, elapsedTime / animationDuration) - 1;
        int currentValue = (int)Mathf.Lerp(startValue, goldValue, progress);

        goldText.text = currentValue.ToString();
        elapsedTime += Time.unscaledDeltaTime;

        yield return null;
    }

    goldText.text = goldValue.ToString();
}



    private IEnumerator AnimateGoldTextRoundStartAttack()
{
    int startValue = attackerGold - attackerGoldIncome;
    goldText.text = startValue.ToString();


    float waitTime = 2f;
    float startTime = Time.realtimeSinceStartup;
    while (Time.realtimeSinceStartup < startTime + waitTime)
    {
        yield return null;
    }


    if (attackerGoldIncome <= 10)
    {
        animationDuration = 0.3f;
    }
    else if (attackerGoldIncome <= 50)
    {
        animationDuration = 0.5f;
    }
    else
    {
        animationDuration = 1f;
    }

    float elapsedTime = 0f;
    float rate = 2.0f;

    while (elapsedTime < animationDuration)
    {

        float progress = Mathf.Pow(rate, elapsedTime / animationDuration) - 1;
        int currentValue = (int)Mathf.Lerp(startValue, attackerGold, progress);

        goldText.text = currentValue.ToString();
        elapsedTime += Time.unscaledDeltaTime;

        yield return null;
    }

    goldText.text = attackerGold.ToString();
}


private IEnumerator AnimateGoldTextRoundStartDefense()
{
    int startValue = defenderGold - defenderGoldIncome;
    goldText.text = startValue.ToString();

    // Wartezeit unabhängig von timeScale
    float waitTime = 2f;
    float startTime = Time.realtimeSinceStartup;
    while (Time.realtimeSinceStartup < startTime + waitTime)
    {
        yield return null;
    }

    // Animation duration based on income
    if (defenderGoldIncome <= 10)
    {
        animationDuration = 0.3f;
    }
    else if (defenderGoldIncome <= 50)
    {
        animationDuration = 0.5f;
    }
    else
    {
        animationDuration = 1f;
    }

    float elapsedTime = 0f;
    float rate = 2f;

    while (elapsedTime < animationDuration)
    {
        // Calculate progress independently of timeScale
        float progress = Mathf.Pow(rate, elapsedTime / animationDuration) - 1;
        int currentValue = (int)Mathf.Lerp(startValue, defenderGold, progress);

        goldText.text = currentValue.ToString();
        elapsedTime += Time.unscaledDeltaTime;

        yield return null; // Wait for the next frame
    }

    // Ensure the final value is set
    goldText.text = defenderGold.ToString();
}


    #endregion 

    #region IncomeAnimation

    public void EnqueueIncomeUpdate(int newIncomeValue, int spending)
    {
        
        updateIncomeQueue.Enqueue(newIncomeValue);
        spendingIncomeQueue.Enqueue(spending);
        

       
        if (!isUpdatingIncome)
        {
            StartCoroutine(ProcessIncomeQueue());
        }
    }

    private IEnumerator ProcessIncomeQueue()
    {
        isUpdatingIncome = true;

        while (updateIncomeQueue.Count > 0)
        {
           
            incomeValue = updateIncomeQueue.Dequeue();
            latestIncomeSpending = spendingIncomeQueue.Dequeue();
            yield return StartCoroutine(UpdateIncomeText());
        }

        isUpdatingIncome = false; 
    }

private IEnumerator UpdateIncomeText()
{
    int startIncomeValue = incomeValue - latestIncomeSpending;


    if (latestIncomeSpending <= 5)
    {
        animationDuration = 0.2f;
    }
    else if (latestIncomeSpending <= 10)
    {
        animationDuration = 0.3f;
    }
    else if (latestIncomeSpending <= 50)
    {
        animationDuration = 0.5f;
    }
    else if (latestIncomeSpending <= 100)
    {
        animationDuration = 1f;
    }
    else
    {
        animationDuration = 1.5f;
    }

    float elapsedTime = 0f;
    float rate = 2.0f;


    while (elapsedTime < animationDuration)
    {

        float progress = Mathf.Pow(rate, elapsedTime / animationDuration) - 1;
        int localIncomeValue = (int)Mathf.Lerp(startIncomeValue, incomeValue, progress);

    
        currentIncomeText.text = "+ " + localIncomeValue.ToString();
        elapsedTime += Time.unscaledDeltaTime;

        yield return null; 
    }

    currentIncomeText.text = "+ " + incomeValue.ToString();
    
    //debug wegen Zwerg
    if(defendersTurn)
    {
        SetIncomeText(defenderGoldIncome);
    }
}

    //Finds all mines in the Dictonary and starts their MineMakesMoney() method
 public void GenerateMineIncome()
 {
     List<GameObject> taggedObjects = new List<GameObject>();

     foreach (GameObject obj in TowerGridPlacement.TowerBible.Values)
     {
         if (obj != null && obj.CompareTag("Mine") && !taggedObjects.Contains(obj))
         {
             taggedObjects.Add(obj);
             //Debug.Log(taggedObjects.Count);
             Mine mineScript = obj.GetComponent<Mine>();
             mineScript.MineMakesMoney();
         }
     }
 }








    #endregion
   
    #region UiAnimation

    public void StartRoundUIAnimation()
    {
        UICanvasAnimator.SetTrigger("RoundStart");

    }

    public void EndRoundUIAnimation()
    {
        UICanvasAnimator.SetTrigger("RoundEnd");
        
    }

    public void ChangeRoundUIAnimation()
    {
        UICanvasAnimator.SetTrigger("RoundChange");
        
    }

    private void PauseButtonAnimationBoolTrue()
    {
        PauseButtonAnimator.SetBool("PauseButton", true);
    }

    public void PauseButtonAnimationBoolFalse()
    {
        PauseButtonAnimator.SetBool("PauseButton", false);
    }

    #endregion

    #region TextUpdates
    public void SetGoldValue(int newValue)
    {
        
        EnqueueGoldUpdate(newValue, lastSpending);
    }

    public void UpdateSupplyText()
    {
        
        supplyText.text = supplyValue.ToString();
    }

    public void SetSupplyValue(int newValue)
    {
        
        supplyValue = newValue;
        UpdateSupplyText();
    }


    public void UpdateMaxSupplyText()
    {
        
        maxSupplyText.text = maxSupplyValue.ToString();
    }

    public void SetMaxSupplyValue(int newValue)
    {
        
        maxSupplyValue = newValue;
        UpdateMaxSupplyText();
    }

        public void UpdateCurrentTurnText()
    {
        
        currentTurnText.text = currentTurnValue.ToString();
    }

    public void SetCurrentTurnValue(int newValue)
    {
        
        currentTurnValue = newValue;
        UpdateCurrentTurnText();
    }

        public void UpdateMaxTurnCountText()
    {
        
        maxTurnCountText.text = maxTurnCountValue.ToString();
    }

    public void SetMaxTurnCountValue(int newValue)
    {
        
        maxTurnCountValue = newValue;
        UpdateMaxTurnCountText();
    }

    public void SetIncomeText(int newValue)
    {
        currentIncomeValue = newValue;
        currentIncomeText.text = "+ " + currentIncomeValue.ToString();
    }

    public void SetCurrentIncomeValue(int newValue)
    {
        currentIncomeValue = newValue;
        EnqueueIncomeUpdate(newValue, incomeBonus);
        
       
    }


    #endregion

    #region Payment

    public void UnitPayment(int unitPrice)
    {
        
        attackerGold = attackerGold - unitPrice;
        lastSpending = unitPrice;
        SetGoldValue(attackerGold);
        
    }

    public void UnitSupplyPayment(int unitPrice)
    {
        
        attackerSupply = attackerSupply + unitPrice;
        SetSupplyValue(attackerSupply);
        
    }


    public void TurretPayment(int turretPrice)
    {
        
        defenderGold = defenderGold - turretPrice;
        lastSpending = turretPrice;
        SetGoldValue(defenderGold);
    }

    public void TurretSupplyPayment(int turretPrice)
    {
        
        defenderSupply = defenderSupply + turretPrice;
        if(!attackersTurn)
        {
        SetSupplyValue(defenderSupply);
        }
        
    }

    public void GainIncomeAttacker(int moreIncome)
    {

        attackerGoldIncome = attackerGoldIncome + moreIncome;
        incomeBonus = moreIncome;
        if(attackersTurn)
        {
        SetCurrentIncomeValue(attackerGoldIncome);
        }

    }

    public void GainIncomeDefender(int moreIncome)
    {

        defenderGoldIncome = defenderGoldIncome + moreIncome;
        incomeBonus = moreIncome;
        SetCurrentIncomeValue(defenderGoldIncome);

    }

    public void GainMaxSupplyAttacker(int moreSupply)
    {
        attackerMaxSupply = attackerMaxSupply + moreSupply;
        SetMaxSupplyValue(attackerMaxSupply);
    }

        public void GainMaxSupplyDefender(int moreSupply)
    {
        defenderMaxSupply = defenderMaxSupply + moreSupply;
        SetMaxSupplyValue(defenderMaxSupply);
    }




    #endregion

    #region GameSettings

    public void UpdateValue(string identifier, int value)
    {
        switch (identifier)
        {
            case "MaxSupply":
                maxSupply = value;
                Debug.Log($"maxSupply wurde auf {maxSupply} gesetzt.");
                break;

            case "StartGoldIncome":
                startGoldIncome = value;
                break;

            case "StartGold":
                defenderStartGold = value;
                attackerStartGold = value;
                Debug.Log($"startgold wurde auf {value} gesetzt.");
                break;

            case "StartGoldAttacker":
                attackerStartGold = value;
                break;

            case "StartGoldDefender":
                defenderStartGold = value;
                break;

            case "TurnCount":
                maxTurnCount = value;
                break;
            case "RepairMultiplier":
                towerRepairCostMultiplier = value;
                Debug.Log($"towerRepairCostMultiplier wurde auf {towerRepairCostMultiplier} gesetzt.");
                break;

            default:
                Debug.LogWarning($"Unbekannter Identifier: {identifier}. Wert wurde nicht gesetzt.");
                break;
        }
    }

    #endregion


    private void EndGameDefenderWin()
    {
        cameraController.MoveCameraToDefenderPosition();
        EndRoundUIAnimation();
        uiManager.ResetTimeScale();

        StartCoroutine(DelayedEndGameDefenderWin());

    }

    private IEnumerator DelayedEndGameDefenderWin()
    {
        yield return new WaitForSecondsRealtime(3f);

        gameInProgress = false;
        gameInMemory = false;
        uiManager.StartOrStopGameUI();
        uiManager.StopAdditionalGameUI();
        ResetGame();

        winnerTextDefender.SetActive(true);
        winnerTextAttacker.SetActive(false);
        imageAttacker.SetActive(false);
        imageDefender.SetActive(true);
        victoryScreen.SetActive(true);
        audioManager.PlayUISound(19);
        audioManager.CrossfadeToClip(7);

    }


    public void EndGameAttackerWin()
    {
        cameraController.MoveCameraToDefenderPosition();
        EndRoundUIAnimation();
        uiManager.ResetTimeScale();
       
        StartCoroutine(DelayedEndGameAttackerWin());
    }

    private IEnumerator DelayedEndGameAttackerWin()
    {

        yield return new WaitForSecondsRealtime(3f);
    
        gameInProgress = false;
        gameInMemory = false;
        uiManager.StartOrStopGameUI();
        uiManager.StopAdditionalGameUI();
        ResetGame();

        winnerTextDefender.SetActive(false);
        winnerTextAttacker.SetActive(true);
        imageAttacker.SetActive(true);
        imageDefender.SetActive(false);
        victoryScreen.SetActive(true);
        audioManager.PlayUISound(20); 
        audioManager.CrossfadeToClip(7);
    }


    public void ResumeGame()
    {
        gameInProgress = true;
        uiManager.TogglePause();

        if(defendersTurn)
        {
            uiManager.ActivateDefenderUI();
            uiManager.ResetTimeScale();
        }
        else if (attackersTurn)
        {
            uiManager.ActivateAttackerUI();
            uiManager.ResetTimeScaleResumeAttacker();
        }
    }

    public void ExitToMainMenuDuringGame()
    {
        gameInProgress = false;
        audioManager.CrossfadeToClip(0); 
    }







    private void ResetIngameValues()
    {
        currentTurn = 1;
        attackerSupply = 0;
        defenderSupply = 0;
        attackersTurn = false;
        defendersTurn = true;
    }

    private void ResetAnimations()
    {
        goldTextAnimator.SetTrigger("ResetAnimationGold");
        goldIncomeTextAnimator.SetTrigger("ResetAnimationIncome");
        attackerRoundTextAnimator.SetTrigger("ResetRoundTextAnimation");
        defenderRoundTextAnimator.SetTrigger("ResetRoundTextAnimation");
        
    }


    public void ResetGame()
    {
        ResetAnimations();
        sequentialActivator.ResetUITierButtons();
        uiManager.ResetNavigation();
        towerGridPlacement.ResetGameTowers();   
        //placeEnemies.ResetGameUnits();
        resettingUnits = true;
        ResetIngameValues();
        cardSelector.ResetAllCards();
        placeEnemies.combinedIncomePerSec = 0;
        
    }

    void LateUpdate()
    {
        if(resettingUnits)
        {
            placeEnemies.ResetGameUnits();
            resettingUnits = false;
        }
    }
}
