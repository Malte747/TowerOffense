using UnityEngine;
using System.Collections.Generic;

public class SequentialActivator : MonoBehaviour
{
    public List<GameObject> objectsToActivate; // Liste der GameObjects
    private int currentIndex = 0; // Der aktuell aktivierte Index

    public void ActivateNextObject()
    {
        // Deaktiviert alle Objekte in der Liste
        foreach (var obj in objectsToActivate)
        {
            obj.SetActive(false);
        }

        // Inkrementiere den Index, und wenn das Ende der Liste erreicht ist, starte von vorn
        currentIndex = (currentIndex + 1) % objectsToActivate.Count;

        // Aktiviere das nächste Objekt in der Liste
        objectsToActivate[currentIndex].SetActive(true);
    }
}
