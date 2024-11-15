using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSettings : MonoBehaviour
{
    [System.Serializable]
    public class SliderInputPair
    {
        public Slider slider;
        public TMP_InputField inputField;
        public Image background;
        public Image border;
        public string identifier;

        public int shortGame;
        public int mediumGame;
        public int longGame;
    }

    public List<SliderInputPair> sliderInputPairs;
    public GameManager gameManager;

    private float sliderMax = 300f;
    private Color overLimitColor = new Color32(96, 94, 94, 255);
    private Color defaultColor = Color.white;

    public void Start()
    {
        foreach (var pair in sliderInputPairs)
        {
            UpdateInputField(pair, pair.slider.value);
            pair.slider.onValueChanged.AddListener(value => UpdateInputField(pair, value));
            pair.inputField.onEndEdit.AddListener(value => UpdateSlider(pair, value));
        }
    }

    public void UpdateAllFields()
    {
        foreach (var pair in sliderInputPairs)
        {
            UpdateInputField(pair, pair.slider.value);
        }
    }

    private void UpdateInputField(SliderInputPair pair, float value)
    {
        if (value < 1)
        {
            value = 1;
        }
        pair.inputField.text = value.ToString("0");
        UpdateImageColors(pair, value <= sliderMax);

        gameManager.UpdateValue(pair.identifier, (int)value);

        UpdateStartGoldValues(pair);
    }

    private void UpdateSlider(SliderInputPair pair, string value)
    {
        if (float.TryParse(value, out float inputValue))
        {
            if (inputValue < 1)
            {
                inputValue = 1;
                UpdateInputField(pair, inputValue);
            }
            if (inputValue > sliderMax)
            {
                UpdateImageColors(pair, false);
            }
            else
            {
                pair.slider.value = inputValue;
                UpdateImageColors(pair, true);
            }

            if (gameManager != null)
            {
                gameManager.UpdateValue(pair.identifier, (int)inputValue);
            }

            UpdateStartGoldValues(pair);
        }
        else
        {
            Debug.LogWarning("Ungültige Eingabe im TMP_InputField.");
        }
    }

    private void UpdateImageColors(SliderInputPair pair, bool withinLimit)
    {
        if (withinLimit)
        {
            pair.background.color = defaultColor;
            pair.border.color = defaultColor;
        }
        else
        {
            pair.background.color = overLimitColor;
            pair.border.color = overLimitColor;
        }
    }

    public void SetAllSlidersToIntValue1()
    {
        foreach (var pair in sliderInputPairs)
        {
            pair.slider.value = Mathf.Clamp(pair.shortGame, 0, sliderMax);
            UpdateInputField(pair, pair.slider.value);
        }
    }

    public void SetAllSlidersToIntValue2()
    {
        foreach (var pair in sliderInputPairs)
        {
            pair.slider.value = Mathf.Clamp(pair.mediumGame, 0, sliderMax);
            UpdateInputField(pair, pair.slider.value);
        }
    }

    public void SetAllSlidersToIntValue3()
    {
        foreach (var pair in sliderInputPairs)
        {
            pair.slider.value = Mathf.Clamp(pair.longGame, 0, sliderMax);
            UpdateInputField(pair, pair.slider.value);
        }
    }

    public void UpdateStartGoldValues(SliderInputPair pair)
    {
        if (pair.identifier == "StartGold")
        {
            float startGoldValue = pair.slider.value;
            foreach (var p in sliderInputPairs)
            {
                if (p.identifier == "StartGoldDefender" || p.identifier == "StartGoldAttacker")
                {
                    p.slider.value = Mathf.Clamp(startGoldValue, 1, sliderMax);
                    UpdateInputField(p, p.slider.value);
                    
                }
            }
        }
    }

public void UpdateImageColorsForStartGold()
    {
        foreach (var pair in sliderInputPairs)
        {
            if (pair.identifier == "StartGold")
            {
                UpdateImageColors(pair, false); 
            }
        }
    }
}
