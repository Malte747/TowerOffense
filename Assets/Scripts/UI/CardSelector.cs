using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelector : MonoBehaviour
{
    UIManager uiManager;
    GameManager gameManager;
    SequentialActivator sequentialActivator;

    [System.Serializable]
    public class Card
    {
        public GameObject prefab;
        public int tier;
        public float spawnChance; // Wahrscheinlichkeit, dass diese Karte ausgewählt wird
        public string cardName; // Eindeutiger Name/Identifier für die Karte
        public int spawnFromRoll; // Der minimale rollCount, ab dem diese Karte spawnen kann
    }

    public List<Card> cards; // Aktuelle Karten im Pool
    private List<Card> cardsBackup; // Backup-Liste der Karten

    public Transform canvasTransform;
    public int currentTier = 1;
    public int rollCount = 0; // Zählt die Anzahl der Aufrufe von CallCards()

    private List<GameObject> spawnedCards = new List<GameObject>();

    private Vector3[] spawnPositions = new Vector3[]
    {
        new Vector3(401, 530, 0),
        new Vector3(960, 530, 0),
        new Vector3(1519, 530, 0)
    };

    public GameObject FarmerPrefab;
    public GameObject[] FarmerSpawnPoints;

    void Start()
    {
        uiManager = GameObject.Find("UiManager").GetComponent<UIManager>();
        sequentialActivator = GameObject.Find("UiManager").GetComponent<SequentialActivator>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Aufrufen der Karten
    public void CallCards()
    {
        if (sequentialActivator.upgradePrice > gameManager.attackerGold)
        {
            return;
        }

        rollCount++; // Erhöhe den rollCount bei jedem Aufruf

        uiManager.TogglePause();
        sequentialActivator.BuyUpgradeAttack();
        uiManager.StartCardSelector();
        DeleteCards(); // Lösche vorherige Karten

        // Nur Karten auswählen, die den aktuellen Tier und den rollCount erfüllen
        List<Card> validCards = cards.FindAll(card => card.tier <= currentTier && card.spawnFromRoll <= rollCount);

        if (validCards.Count < spawnPositions.Length)
        {
            Debug.LogError("Nicht genügend Karten verfügbar, um alle Positionen zu füllen.");
            return;
        }

        StartCoroutine(SpawnCardsWithDelay(validCards));
    }

    // Coroutine zum verzögerten Spawnen der Karten
    private IEnumerator SpawnCardsWithDelay(List<Card> validCards)
    {
        List<Card> selectedCards = new List<Card>();

        for (int i = 0; i < spawnPositions.Length; i++)
        {
            Card selectedCard = SelectCardBasedOnChance(validCards, selectedCards);
            if (selectedCard != null)
            {
                GameObject cardPrefab = selectedCard.prefab;
                GameObject spawnedCard = Instantiate(cardPrefab, spawnPositions[i], Quaternion.identity, canvasTransform);
                spawnedCards.Add(spawnedCard);
                selectedCards.Add(selectedCard);
            }

            // Warte 0,2 Sekunden (unscaled time)
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    // Auswahl einer Karte basierend auf der spawnChance und Vermeidung von Duplikaten
    private Card SelectCardBasedOnChance(List<Card> validCards, List<Card> selectedCards)
    {
        List<Card> availableCards = new List<Card>(validCards);
        foreach (Card selectedCard in selectedCards)
        {
            availableCards.Remove(selectedCard);
        }

        if (availableCards.Count == 0)
        {
            Debug.LogError("Keine verfügbaren Karten zum Spawnen.");
            return null;
        }

        float totalChance = 0f;
        foreach (Card card in availableCards)
        {
            totalChance += card.spawnChance;
        }

        float randomValue = Random.Range(0f, totalChance);
        float accumulatedChance = 0f;

        foreach (Card card in availableCards)
        {
            accumulatedChance += card.spawnChance;
            if (randomValue <= accumulatedChance)
            {
                return card;
            }
        }

        return availableCards[availableCards.Count - 1];
    }

    // Löscht alle derzeitigen Karten
    public void DeleteCards()
    {
        foreach (GameObject card in spawnedCards)
        {
            if (card != null)
            {
                Destroy(card);
            }
        }
        spawnedCards.Clear();
    }

    // Entfernt eine Karte aus dem Pool basierend auf ihrem "cardName" (String)
    public void RemoveCardFromPool(string cardName)
    {
        Card cardToRemove = cards.Find(card => card.cardName == cardName);
        if (cardToRemove != null)
        {
            cards.Remove(cardToRemove);
            Debug.Log($"Karte mit dem Namen {cardName} wurde aus dem Pool entfernt.");
        }
        else
        {
            Debug.LogWarning($"Karte mit dem Namen {cardName} wurde im Pool nicht gefunden.");
        }
    }

    // Setze alle Karten wieder auf den ursprünglichen Zustand zurück
    public void ResetAllCards()
    {
        if (cardsBackup == null || cardsBackup.Count == 0)
        {
            cardsBackup = new List<Card>(cards);
            Debug.Log("Karten wurden gesichert.");
        }

        cards.Clear();
        cards.AddRange(cardsBackup);
        currentTier = 1;
        rollCount = 0; // Setze den rollCount zurück
        Debug.Log("Alle Karten wurden zurückgesetzt.");
    }

    public void TierUp()
    {
        currentTier++;
    }

    public void SpawnFarmerPrefabs(int numberOfPrefabsToSpawn)
    {
        if (FarmerSpawnPoints.Length == 0)
        {
            Debug.LogError("Keine Spawnpunkte definiert!");
            return;
        }

        if (numberOfPrefabsToSpawn > FarmerSpawnPoints.Length)
        {
            Debug.LogError("Zu viele Prefabs angegeben! Es gibt nicht genügend einzigartige Spawnpunkte.");
            return;
        }

        List<GameObject> availableSpawnPoints = new List<GameObject>(FarmerSpawnPoints);

        for (int i = 0; i < numberOfPrefabsToSpawn; i++)
        {
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            Instantiate(FarmerPrefab, availableSpawnPoints[randomIndex].transform.position, availableSpawnPoints[randomIndex].transform.rotation);
            availableSpawnPoints.RemoveAt(randomIndex);
        }

        Debug.Log($"{numberOfPrefabsToSpawn} Farmer wurden zufällig an einzigartigen Positionen gespawnt.");
    }




    #region Gamba Methods Tower

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





    #endregion
}
