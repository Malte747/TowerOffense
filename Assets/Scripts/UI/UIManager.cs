using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    GameManager gameManager;

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

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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

}
