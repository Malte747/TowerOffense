using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private int goldProduced;
        
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.GainIncomeDefender(goldProduced);
    }

    // Update is called once per frame
    public void MineIsDying()
    {
        gameManager.GainIncomeDefender(- goldProduced);
        Debug.Log("Mine is not ALive");
    }
}
