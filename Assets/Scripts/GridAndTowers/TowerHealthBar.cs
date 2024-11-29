using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerHealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthBarSprite;
    [SerializeField] private RectTransform _healthBarCanvas;

    private Health _health;

    private Camera _camera;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        _health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion lookAtCamera = Quaternion.LookRotation(transform.position - _camera.transform.position);
        _healthBarCanvas.transform.rotation = lookAtCamera;

    }

    public void UpdateHealthBar(int maxHealth, float currentHealth)
    {
        if (currentHealth / maxHealth < 1) { _healthBarCanvas.gameObject.SetActive(true);}
        else if (currentHealth / maxHealth >= 1) { _healthBarCanvas.gameObject.SetActive(false); }
        //Debug.Log(currentHealth + " a " + maxHealth + " a " + currentHealth / maxHealth);
        _healthBarSprite.fillAmount = currentHealth / maxHealth;
        UIManager uiManager = GameObject.Find("UiManager").GetComponent<UIManager>();
        uiManager.SetTowerHPSliderUIValues(maxHealth, Mathf.RoundToInt(currentHealth));
    }
}
