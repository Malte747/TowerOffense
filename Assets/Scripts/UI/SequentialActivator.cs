using UnityEngine;
using System.Collections.Generic;

public class SequentialActivator : MonoBehaviour
{
    public List<GameObject> objectsToActivateDefense; // Liste der GameObjects
    public List<GameObject> objectsToActivateAttack;
    private int currentIndex = 0; // Der aktuell aktivierte Index

    public void ActivateNextObject()
    {
        // Deaktiviert alle Objekte in der Liste
        foreach (var obj in objectsToActivateDefense)
        {
            obj.SetActive(false);
        }

        // Inkrementiere den Index, und wenn das Ende der Liste erreicht ist, starte von vorn
        currentIndex = (currentIndex + 1) % objectsToActivateDefense.Count;

        // Aktiviere das nächste Objekt in der Liste
        objectsToActivateDefense[currentIndex].SetActive(true);
    }

        public void ActivateNextObjectAttack()
    {
        // Deaktiviert alle Objekte in der Liste
        foreach (var obj in objectsToActivateAttack)
        {
            obj.SetActive(false);
        }

        // Inkrementiere den Index, und wenn das Ende der Liste erreicht ist, starte von vorn
        currentIndex = (currentIndex + 1) % objectsToActivateAttack.Count;

        // Aktiviere das nächste Objekt in der Liste
        objectsToActivateAttack[currentIndex].SetActive(true);
    }
}
