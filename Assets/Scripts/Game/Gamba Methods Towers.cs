using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GambaMethodsTowers : MonoBehaviour
{
    //Finds all Towers in the Dictonary and destroys a Random one
    public void DestroyTowers(int amount)
    {
        List<GameObject> taggedObjects = new List<GameObject>();

        foreach (GameObject obj in TowerGridPlacement.TowerBible.Values)
        {
            if (obj != null && obj.CompareTag("Tower") && !taggedObjects.Contains(obj))
            {
                taggedObjects.Add(obj);
                //Debug.Log(taggedObjects.Count);
            }
        }
        if (taggedObjects.Count > 0)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject towerToDestroy = taggedObjects[Random.Range(1, taggedObjects.Count)];
                taggedObjects.Remove(towerToDestroy);
                towerToDestroy.GetComponent<HealthTowers>().Death();
            }
        }
        else if (taggedObjects.Count > 0)
        {
            foreach (GameObject towerToDestroy in taggedObjects)
            {
                towerToDestroy.GetComponent<HealthTowers>().Death();
            }
        }
        else Debug.Log("No Towers found");
    }

    //Finds all Towers in the dictonary and removes a specic percent of current HP (cannot reduce HP below 0)
    public void TowersLooseHP(int percentage)
    {
        List<GameObject> taggedObjects = new List<GameObject>();

        foreach (GameObject obj in TowerGridPlacement.TowerBible.Values)
        {
            if (obj != null && obj.CompareTag("Tower") && !taggedObjects.Contains(obj))
            {
                taggedObjects.Add(obj);
                //Debug.Log(taggedObjects.Count);
                HealthTowers towerHealth = obj.GetComponent<HealthTowers>();
                int hpToLoose = Mathf.RoundToInt((towerHealth.health * percentage) / 100);
                towerHealth.health -= hpToLoose;
                if (towerHealth.health < 1) //Makes this effect be unable to kill a Tower
                {
                    towerHealth.health = 1;
                }
            }
        }

    }


    //Finds all mines in the Dictonary and destroys a Random one
    public void DestroyMines(int amount)
    {
        List<GameObject> taggedObjects = new List<GameObject>();

        foreach (GameObject obj in TowerGridPlacement.TowerBible.Values)
        {
            if (obj != null && obj.CompareTag("Mine") && !taggedObjects.Contains(obj))
            {
                taggedObjects.Add(obj);
                //Debug.Log(taggedObjects.Count);
            }
        }
        if (taggedObjects.Count > amount)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject mineToDestroy = taggedObjects[Random.Range(1,taggedObjects.Count)];
                taggedObjects.Remove(mineToDestroy);
                mineToDestroy.GetComponent<HealthTowers>().Death();
            }
        }
        else if (taggedObjects.Count > 0)
        {
            foreach (GameObject mineToDestroy in taggedObjects)
            {
                mineToDestroy.GetComponent<HealthTowers>().Death();
            }
        }
        else Debug.Log("No Mines found");
    }
}
