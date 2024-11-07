using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public MultiButtonTrigger multiButtonTriggerAttack;
    public MultiButtonTrigger multiButtonTriggerDefense;


    public GameObject attackerUIBlock;
    public GameObject defenderUIBlock;
    public GameObject attackerSupplyIcon;
    public GameObject defenderSupplyIcon;

    public Button pause;  
    public Button unpause;
    private bool isPaused = false; 
    private bool toggle = true;    



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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


    }

    public void ResetNavigation()
    {
        multiButtonTriggerAttack.ResetMenuNavigation();
        multiButtonTriggerDefense.ResetMenuNavigation();
        attackerUIBlock.SetActive(false);
        defenderUIBlock.SetActive(true);
    }

}
