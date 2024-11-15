using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    UIManager uiManager;
    SequentialActivator sequentialActivator;



    //Turns
    [Header("Turns")]

    [SerializeField] private bool attackersTurn = false;
    [SerializeField] private bool defendersTurn = true;
    [SerializeField] private int currentTurn = 1;
    public int maxTurnCount;



    //Attackers stats
    [Header("Attacker Stats")]

    public static int attackerGold;
    public int attackerStartGold;
    [SerializeField] private int attackerGoldIncome;
    public static int attackerSupply;



    //defenders stats
    [Header("Defender Stats")]

    public static int defenderGold; 
    public int defenderStartGold;
    [SerializeField] private int defenderGoldIncome;
    public static int defenderSupply;


    //Allgemeine stats
    [Header("Allgemeine Stats")]

    public static int maxSupply;
    public int startGoldIncome;



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
  
    




    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        sequentialActivator = GameObject.Find("UiManager").GetComponent<SequentialActivator>();

        GameStart();
    }

    public void GameStart()
    {
        attackerGold = attackerStartGold;
        attackerGoldIncome = startGoldIncome;
        

        defenderGold = defenderStartGold;
        defenderGoldIncome = startGoldIncome;

        SetMaxSupplyValue(maxSupply);
        SetCurrentTurnValue(currentTurn);
        SetMaxTurnCountValue(maxTurnCount);
        



        DefendersTurn();
    }


    private void AttackersTurn()
    {
        if (attackersTurn)
        {
            attackerGold = attackerGold + attackerGoldIncome;
            goldTextAnimator.SetTrigger("GoldAnimation");
            goldIncomeTextAnimator.SetTrigger("IncomeAnimation");
            StartCoroutine(AnimateGoldTextRoundStartAttack());
            SetSupplyValue(attackerSupply);
            SetIncomeText(attackerGoldIncome);
            roundTextAttackObject.SetActive(true);
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
            roundTextDefenseObject.SetActive(true);
            
        }
    }

    public void EndTrun()
    {
        if(maxTurnCount == currentTurn && attackersTurn)
        {
            EndGameDefenderWin();
        }
        else if (attackersTurn)
        {
            attackersTurn = false;
            defendersTurn = true;
            currentTurn++;
            SetCurrentTurnValue(currentTurn);
            uiManager.ActivateDefenderUI();
            roundTextAttackObject.SetActive(false);
            DefendersTurn();
        }
        else if (defendersTurn)
        {
            defendersTurn = false;
            attackersTurn = true;
            uiManager.ActivateAttackerUI();
            roundTextDefenseObject.SetActive(false);
            AttackersTurn();
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
        if(latestSpending <= 10)
        {
            animationDuration = 0.3f;
        }
        else if (latestSpending <= 50)
        {
            animationDuration = 0.5f;
        }
        else if(latestSpending <= 100)
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
            int currentValue = (int)Mathf.Lerp(startValue, goldValue, progress);

            goldText.text = currentValue.ToString();
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        goldText.text = goldValue.ToString();
        
    }


    private IEnumerator AnimateGoldTextRoundStartAttack()
    {
        int startValue = attackerGold - attackerGoldIncome;
        goldText.text = startValue.ToString();

        yield return new WaitForSeconds(2f);

        

        if(attackerGoldIncome <= 10)
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
            elapsedTime += Time.deltaTime;
            
            yield return null; 
        }
        
        
        goldText.text = attackerGold.ToString();
    }

    private IEnumerator AnimateGoldTextRoundStartDefense()
    {
        int startValue = defenderGold - defenderGoldIncome;
        goldText.text = startValue.ToString();

        yield return new WaitForSeconds(2f);


        if(defenderGoldIncome <= 10)
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
            
            float progress = Mathf.Pow(rate, elapsedTime / animationDuration) - 1;
            int currentValue = (int)Mathf.Lerp(startValue, defenderGold, progress);
            
            goldText.text = currentValue.ToString();
            elapsedTime += Time.deltaTime;
            
            yield return null; 
        }
        
        
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
        if(latestIncomeSpending <= 5)
        {
            animationDuration = 0.2f;
        }
        else if(latestIncomeSpending <= 10)
        {
            animationDuration = 0.3f;
        }
        else if (latestIncomeSpending <= 50)
        {
            animationDuration = 0.5f;
        }
        else if(latestIncomeSpending <= 100)
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
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        currentIncomeText.text = "+ " + currentIncomeValue.ToString();
        
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
        SetSupplyValue(defenderSupply);
        
    }

    public void GainIncomeAttacker(int moreIncome)
    {

        attackerGoldIncome = attackerGoldIncome + moreIncome;
        incomeBonus = moreIncome;
        SetCurrentIncomeValue(attackerGoldIncome);

    }

    public void GainIncomeDefender(int moreIncome)
    {

        defenderGoldIncome = defenderGoldIncome + moreIncome;
        incomeBonus = moreIncome;
        SetCurrentIncomeValue(defenderGoldIncome);

    }



    #endregion




    private void EndGameDefenderWin()
    {
        Debug.Log("Das Spiel ist zuende, verteidiger gewinnt");
    }







        private void ResetIngameValues()
    {
        currentTurn = 1;
        attackerSupply = 0;
        defenderSupply = 0;
        attackersTurn = false;
        defendersTurn = true;
    }


    public void ResetGame()
    {
        sequentialActivator.ResetUITierButtons();
        uiManager.ResetNavigation();
        ResetIngameValues();
    }


}
