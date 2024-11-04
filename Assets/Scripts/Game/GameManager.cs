using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    UIManager uiManager;



    //Turns

    [SerializeField] private bool attackersTurn = false;
    [SerializeField] private bool defendersTurn = true;

    //Attackers stats




    //defenders stats




    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }


    private void AttackersTurn()
    {
        if (attackersTurn)
        {

        }
    }

    private void DefendersTurn()
    {
        if (defendersTurn)
        {
            
        }
    }



    public void EndTrun()
    {
        if (attackersTurn)
        {
            attackersTurn = false;
            defendersTurn = true;
            uiManager.ActivateDefenderUI();
        }
        else if (defendersTurn)
        {
            defendersTurn = false;
            attackersTurn = true;
            uiManager.ActivateAttackerUI();
        }
    }


}
