using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public MonoBehaviour defenderUI;
    public MonoBehaviour attackerUI;
    public GameObject attackerUIBlock;
    public GameObject defenderUIBlock;

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
        attackerUI.enabled = false;
        defenderUI.enabled = true;
        attackerUIBlock.SetActive(false);
        defenderUIBlock.SetActive(true);
    }

    public void ActivateAttackerUI()
    {
        defenderUI.enabled = false;
        attackerUI.enabled = true;
        defenderUIBlock.SetActive(false);
        attackerUIBlock.SetActive(true);
    }

}
