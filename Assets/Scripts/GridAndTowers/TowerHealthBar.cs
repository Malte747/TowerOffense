                    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerHealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform _healthBarCanvas;
    [SerializeField] private Slider _healthBarSlider;

    private HealthTowers _health;



    // Start is called before the first frame update
    void Start()
    {
        _health = GetComponent<HealthTowers>();
        if(_health.TowerStats.target == TowerStats.Targets.MainTower)
        {
            _healthBarSlider = GameObject.Find("HealthBar_MainTower").GetComponent<Slider>();
            Debug.LogWarning(_healthBarSlider);
        }
        else
        {
            _healthBarSlider = _healthBarCanvas.GetComponentInChildren<Slider>();
        }
    }



    public void UpdateHealthBar(int maxHealth, float currentHealth)
    {
        if (currentHealth / maxHealth < 1 && _health.TowerStats.target != TowerStats.Targets.MainTower) 
        {
            _healthBarCanvas.gameObject.SetActive(true);
        }
        else if (currentHealth / maxHealth >= 1 && _health.TowerStats.target != TowerStats.Targets.MainTower) 
        { 
            _healthBarCanvas.gameObject.SetActive(false); 
        }
        //Debug.Log(currentHealth + " a " + maxHealth + " a " + currentHealth / maxHealth);
        _healthBarSlider.value = currentHealth / maxHealth;
        UIManager uiManager = GameObject.Find("UiManager").GetComponent<UIManager>();
        uiManager.SetTowerHPSliderUIValues(maxHealth, Mathf.RoundToInt(currentHealth));
    }
}
