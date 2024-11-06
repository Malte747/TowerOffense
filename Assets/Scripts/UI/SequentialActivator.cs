using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SequentialActivator : MonoBehaviour
{
    public List<GameObject> objectsToActivateDefense; // Liste der GameObjects
    public List<GameObject> objectsToActivateTierDefense;
    public List<GameObject> objectsToActivateAttack;
    public List<GameObject> objectsToActivateTierAttack;
    private int currentIndex = 0; // Der aktuell aktivierte Index
    private int currentIndexAttack = 0;

    public void ActivateNextObject()
    {
        if(currentIndex + 2 <= objectsToActivateDefense.Count)
        {

        foreach (var obj in objectsToActivateDefense)
        {
            obj.SetActive(false);
        }
        foreach (var obj in objectsToActivateTierDefense)
        {
            SetButtonsInteractable(obj, false);
        }


        currentIndex = (currentIndex + 1) % objectsToActivateDefense.Count;


        objectsToActivateDefense[currentIndex].SetActive(true);


        for (int i = 0; i <= currentIndex; i++)
        {
            SetButtonsInteractable(objectsToActivateTierDefense[i], true);
        }
        }
    }

        public void ActivateNextObjectAttack()
    {
        if(currentIndexAttack + 2 <= objectsToActivateDefense.Count)
        {
        // Deaktiviert alle Objekte in der Liste
        foreach (var obj in objectsToActivateAttack)
        {
            obj.SetActive(false);
        }
        foreach (var obj in objectsToActivateTierAttack)
        {
            SetButtonsInteractable(obj, false);
        }

        // Inkrementiere den Index, und wenn das Ende der Liste erreicht ist, starte von vorn
        currentIndexAttack = (currentIndexAttack + 1) % objectsToActivateAttack.Count;

        // Aktiviere das nächste Objekt in der Liste
        objectsToActivateAttack[currentIndexAttack].SetActive(true);

            for (int i = 0; i <= currentIndexAttack; i++)
        {
            SetButtonsInteractable(objectsToActivateTierAttack[i], true);
        }
        }
    }

private void SetButtonsInteractable(GameObject obj, bool interactable)
{
    Button[] buttons = obj.GetComponentsInChildren<Button>(true);
    foreach (Button button in buttons)
    {
        button.interactable = interactable;

        // Setze den Tag basierend auf dem Zustand des Buttons
        if (interactable)
        {
            button.tag = "TierButtonActive"; // Tag ändern, wenn der Button aktiviert ist
        }
        else
        {
            button.tag = "TierButtonInactive"; // Tag zurücksetzen, wenn der Button deaktiviert ist
        }
    }
}



public void ResetUITierButtons()
{
    // Setze nur die Buttons der Objekte mit Index 1 und 2 in Tier-Listen auf "TierButtonInactive" und deaktiviere sie
    if (objectsToActivateTierDefense.Count > 1)
    {
        SetButtonsInteractable(objectsToActivateTierDefense[1], false);
    }
    if (objectsToActivateTierDefense.Count > 2)
    {
        SetButtonsInteractable(objectsToActivateTierDefense[2], false);
    }

    if (objectsToActivateTierAttack.Count > 1)
    {
        SetButtonsInteractable(objectsToActivateTierAttack[1], false);
    }
    if (objectsToActivateTierAttack.Count > 2)
    {
        SetButtonsInteractable(objectsToActivateTierAttack[2], false);
    }

    if (objectsToActivateDefense.Count > 1)
    {
        objectsToActivateDefense[1].SetActive(false);
    }
    if (objectsToActivateDefense.Count > 2)
    {
        objectsToActivateDefense[2].SetActive(false);
    }

    if (objectsToActivateAttack.Count > 1)
    {
        objectsToActivateAttack[1].SetActive(false);
    }
    if (objectsToActivateAttack.Count > 2)
    {
        objectsToActivateAttack[2].SetActive(false);
    }

    // Aktiviere das Element an Index 0 in Defense- und Attack-Listen
    if (objectsToActivateDefense.Count > 0)
    {
        objectsToActivateDefense[0].SetActive(true);
    }
    if (objectsToActivateAttack.Count > 0)
    {
        objectsToActivateAttack[0].SetActive(true);
    }

    // Zurücksetzen der Indizes
    currentIndex = 0;
    currentIndexAttack = 0;
}
}

