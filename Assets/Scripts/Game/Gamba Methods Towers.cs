using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GambaMethodsTowers : MonoBehaviour
{
    //Finds all Tag matching Buildings in the Dictonary and destroys a Random one
    public void DestroyBuilding(int amount, string tagString)
    {
        List<GameObject> taggedObjects = new List<GameObject>();

        foreach (GameObject obj in TowerGridPlacement.TowerBible.Values)
        {
            if (obj != null && obj.CompareTag(tagString) && !taggedObjects.Contains(obj))
            {
                taggedObjects.Add(obj);
                //Debug.Log(taggedObjects.Count);
            }
        }
        if (taggedObjects.Count > 0)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject buildingToDestroy = taggedObjects[Random.Range(1, taggedObjects.Count)];
                taggedObjects.Remove(buildingToDestroy);
                buildingToDestroy.GetComponent<HealthTowers>().Death();
            }
        }
        else if (taggedObjects.Count > 0)
        {
            foreach (GameObject buildingToDestroy in taggedObjects)
            {
                buildingToDestroy.GetComponent<HealthTowers>().Death();
            }
        }
        else Debug.Log("No matching Buildings found");
    }

    //Finds all Tag matching Buildings in the dictonary and removes a specic percent of current HP (cannot reduce HP below 0)
    public void TowersLooseHP(int percentage, string tagString)
    {
        List<GameObject> taggedObjects = new List<GameObject>();

        foreach (GameObject obj in TowerGridPlacement.TowerBible.Values)
        {
            if (obj != null && obj.CompareTag(tagString) && !taggedObjects.Contains(obj))
            {
                taggedObjects.Add(obj);
                //Debug.Log(taggedObjects.Count);
                HealthTowers towerHealth = obj.GetComponent<HealthTowers>();
                int hpToLoose = Mathf.RoundToInt((towerHealth.health * percentage) / 100);
                towerHealth.health -= hpToLoose;
                if (towerHealth.health < 1) //Makes this effect be unable to kill a Building
                {
                    towerHealth.health = 1;
                }
            }
        }
    }
}