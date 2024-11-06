using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MultiButtonTrigger : MonoBehaviour
{
    [System.Serializable]
    public struct KeyButtonPair
    {
        public KeyCode key;
        public Button button;
    }

    [System.Serializable]
    public class Menu
    {
        public string menuName;                     // Name des Menüs
        public List<KeyButtonPair> keyButtonPairs;   // Liste der Tasten-Button-Paare
    }

    public List<Menu> menus;                        // Liste aller Menüs (Hauptmenü und Untermenüs)
    private Menu activeMenu;                        // Das aktuell aktive Menü
    private Dictionary<KeyCode, Button> buttonMapping = new Dictionary<KeyCode, Button>();
    private bool menuChanged = false;               // Markierung, ob das Menü gewechselt wurde

    // Liste der Submemüs für die reset funktion
    public List<GameObject> TierMenues;

    void Start()
    {
        // Standardmäßig das erste Menü als aktiv setzen (z. B. das Hauptmenü)
        if (menus.Count > 0)
        {
            SetActiveMenu(menus[0]);
        }
    }

    void Update()
    {
        // Überprüfe, ob das Menü geändert wurde und aktualisiere die Tastenliste einmalig
        if (menuChanged)
        {
            UpdateButtonMapping();
            menuChanged = false;
        }

        // Überprüfe die Tasten für das aktive Menü
        foreach (var pair in buttonMapping)
        {
            if (Input.GetKeyDown(pair.Key))
            {
                HandleButtonPress(pair.Value);
            }

            if (Input.GetKeyUp(pair.Key))
            {
                ResetButtonColor(pair.Value);
            }
        }
    }

    void SetActiveMenu(Menu menu)
    {
        // Deaktiviere alle Buttons in den bisherigen Menüs
        foreach (var m in menus)
        {
            foreach (var pair in m.keyButtonPairs)
            {
                pair.button.interactable = false; // Alle Buttons in anderen Menüs deaktivieren
            }
        }

        // Setze das neue aktive Menü und markiere die Änderung
        activeMenu = menu;
        menuChanged = true; // Markiere das Menü als geändert, damit UpdateButtonMapping aufgerufen wird

        // Aktiviere die Buttons im neuen aktiven Menü
foreach (var pair in activeMenu.keyButtonPairs)
{
    // Prüfe, ob der Button nicht den Tag "TierButton" hat
    if (pair.button.tag != "TierButtonInactive")
    {
        pair.button.interactable = true; // Nur Buttons ohne den Tag "TierButton" interaktiv machen
    }
    else
    {
        pair.button.interactable = false; // Deaktiviere Buttons mit dem Tag "TierButton"
    }
}
    }

    void UpdateButtonMapping()
    {
        buttonMapping.Clear(); // Button-Zuordnung zurücksetzen

        // Fülle das buttonMapping für das aktuelle Menü
        foreach (var pair in activeMenu.keyButtonPairs)
        {
            buttonMapping[pair.key] = pair.button;
        }
    }

    void HandleButtonPress(Button button)
    {
        // Überprüfen, ob der Button interaktiv ist
        if (!button.interactable) return;

        // Setze den Button auf die gedrückte Farbe
        var colors = button.colors;
        button.image.color = colors.pressedColor;

        // Löse das onClick-Event des Buttons aus
        button.onClick.Invoke();

        // Überprüfen, ob der Button ein Untermenü öffnen soll
        Menu submenu = menus.Find(m => m.menuName == button.name);
        if (submenu != null)
        {
            SetActiveMenu(submenu);
        }
    }

    public void ResetButtonColor(Button button)
    {
        // Setze die Farbe auf normal zurück
        var colors = button.colors;
        button.image.color = colors.normalColor;
    }

    public void SwitchMenuByButtonClick(Button button)
    {
        // Suche das Untermenü, das dem Buttonnamen entspricht
        Menu submenu = menus.Find(m => m.menuName == button.name);

        // Wenn ein entsprechendes Untermenü gefunden wird, aktiviere es
        if (submenu != null)
        {
            SetActiveMenu(submenu);
        }
    }

    public void ResetMenuNavigation()
    {
        
        SetActiveMenu(menus[0]);

         for (int i = 0; i < TierMenues.Count; i++)
            {
                // Aktiviert nur das erste Element (Index 0), deaktiviert alle anderen
                if (i == 0)
                {
                    TierMenues[i].SetActive(true); // Element 0 aktivieren
                }
                else
                {
                    TierMenues[i].SetActive(false); // Alle anderen deaktivieren
                }
            }
    }
}
