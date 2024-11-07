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

    public int attackerGold;
    public int attackerStartGold;
    [SerializeField] private int attackerGoldIncome;
    public int attackerSupply;



    //defenders stats
    [Header("Defender Stats")]

    public int defenderGold;
    public int defenderStartGold;
    [SerializeField] private int defenderGoldIncome;
    public int defenderSupply;


    //Allgemeine stats
    [Header("Allgemeine Stats")]

    public int maxSupply;
    public int startGoldIncome;

    // Text
    [Header("Texts")]

    [SerializeField] public TMP_Text goldText;
    [SerializeField] public TMP_Text currentIncomeText;
    [SerializeField] public TMP_Text supplyText;
    [SerializeField] public TMP_Text maxSupplyText;
    [SerializeField] public TMP_Text currentTurnText;
    [SerializeField] public TMP_Text maxTurnCountText;
    private int goldValue; 
    private int currentIncomeValue; 
    private int supplyValue;
    private int maxSupplyValue;
    private int currentTurnValue;
    private int maxTurnCountValue;
    




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
            SetGoldValue(attackerGold);
            SetSupplyValue(attackerSupply);
            SetCurrentIncomeValue(attackerGoldIncome);
        }
    }

    private void DefendersTurn()
    {
        if (defendersTurn)
        {
            defenderGold = defenderGold + defenderGoldIncome;
            SetGoldValue(defenderGold);
            SetSupplyValue(defenderSupply);
            SetCurrentIncomeValue( defenderGoldIncome);
            
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
            DefendersTurn();
        }
        else if (defendersTurn)
        {
            defendersTurn = false;
            attackersTurn = true;
            uiManager.ActivateAttackerUI();
            AttackersTurn();
        }
    }




    #region TextUpdates

        public void UpdateGoldText()
    {
        
        goldText.text = goldValue.ToString();
    }

    public void SetGoldValue(int newValue)
    {
        
        goldValue = newValue;
        UpdateGoldText();
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

    public void UpdateCurrentIncomeText()
    {
        
        currentIncomeText.text = "+ " + currentIncomeValue.ToString();
    }

    public void SetCurrentIncomeValue(int newValue)
    {
        
        currentIncomeValue = newValue;
        UpdateCurrentIncomeText();
    }


    #endregion

    #region Payment

    public void UnitPayment(int unitPrice)
    {
        
        attackerGold = attackerGold - unitPrice;
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
        SetCurrentIncomeValue(attackerGoldIncome);

    }

    public void GainIncomeDefender(int moreIncome)
    {

        defenderGoldIncome = defenderGoldIncome + moreIncome;
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
