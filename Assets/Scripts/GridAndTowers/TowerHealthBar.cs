using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerHealthBar : MonoBehaviour
{
    [SerializeField] private Slider _healthBarSlider;
    [SerializeField] private RectTransform _healthBarCanvas;

    private Health _health;



    // Start is called before the first frame update
    void Start()
    {

        _health = GetComponent<Health>();
    }



    public void UpdateHealthBar(int maxHealth, float currentHealth)
    {
        if (currentHealth / maxHealth < 1) { _healthBarCanvas.gameObject.SetActive(true);}
        else if (currentHealth / maxHealth >= 1) { _healthBarCanvas.gameObject.SetActive(false); }
        //Debug.Log(currentHealth + " a " + maxHealth + " a " + currentHealth / maxHealth);
        _healthBarSlider.value = currentHealth / maxHealth;
        UIManager uiManager = GameObject.Find("UiManager").GetComponent<UIManager>();
        uiManager.SetTowerHPSliderUIValues(maxHealth, Mathf.RoundToInt(currentHealth));
    }
}
