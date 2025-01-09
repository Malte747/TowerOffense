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
    }

    public List<Card> cards; // Aktuelle Karten im Pool
    private List<Card> cardsBackup; // Backup-Liste der Karten

    public Transform canvasTransform;
    public int currentTier = 1;

    private List<GameObject> spawnedCards = new List<GameObject>();

    private Vector3[] spawnPositions = new Vector3[] 
    {
        new Vector3(401, 530, 0),
        new Vector3(960, 530, 0),
        new Vector3(1519, 530, 0)
    };

    // Farmer Revolt

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
        
        uiManager.TogglePause();
        sequentialActivator.BuyUpgradeAttack();
        uiManager.StartCardSelector();
        DeleteCards(); // Lösche vorherige Karten

        // Nur Karten mit dem Tier <= currentTier auswählen
        List<Card> validCards = cards.FindAll(card => card.tier <= currentTier);

        if (validCards.Count < spawnPositions.Length)
        {
            Debug.LogError("Nicht genügend Karten verfügbar, um alle Positionen zu füllen.");
            return;
        }

        // Karten ohne Wiederholungen spawnieren
        List<Card> selectedCards = new List<Card>();

        for (int i = 0; i < spawnPositions.Length; i++)
        {
            Card selectedCard = SelectCardBasedOnChance(validCards, selectedCards);
            GameObject cardPrefab = selectedCard.prefab;
            GameObject spawnedCard = Instantiate(cardPrefab, spawnPositions[i], Quaternion.identity, canvasTransform);
            spawnedCards.Add(spawnedCard);
            selectedCards.Add(selectedCard); // Karte zum 'selectedCards' hinzufügen, um Wiederholungen zu vermeiden
        }
    }

    // Auswahl einer Karte basierend auf der spawnChance und Vermeidung von Duplikaten
    private Card SelectCardBasedOnChance(List<Card> validCards, List<Card> selectedCards)
    {
        // Filtern der bereits ausgewählten Karten
        List<Card> availableCards = new List<Card>(validCards);
        foreach (Card selectedCard in selectedCards)
        {
            availableCards.Remove(selectedCard); // Entfernen der bereits ausgewählten Karten
        }

        if (availableCards.Count == 0)
        {
            Debug.LogError("Keine verfügbaren Karten zum Spawnen.");
            return null;
        }

        // Berechnung der Gesamt-Wahrscheinlichkeit
        float totalChance = 0f;
        foreach (Card card in availableCards)
        {
            totalChance += card.spawnChance;
        }

        // Zufallswert generieren
        float randomValue = Random.Range(0f, totalChance);
        float accumulatedChance = 0f;

        foreach (Card card in availableCards)
        {
            accumulatedChance += card.spawnChance;
            if (randomValue <= accumulatedChance)
            {
                return card; // Rückgabe der ausgewählten Karte
            }
        }

        // Falls keine Karte ausgewählt wurde, gebe die letzte zurück (Sicherheitsnetz)
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
            // Wenn noch keine Backup-Liste vorhanden ist, erstellen wir eine
            cardsBackup = new List<Card>(cards);
            Debug.Log("Karten wurden gesichert.");
        }

        // Karten wiederherstellen, falls sie entfernt wurden
        cards.Clear();
        cards.AddRange(cardsBackup);
        currentTier = 1;
        currentTier = 1;
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

    // Erstelle eine Liste der verfügbaren Spawnpunkte
    List<GameObject> availableSpawnPoints = new List<GameObject>(FarmerSpawnPoints);

    for (int i = 0; i < numberOfPrefabsToSpawn; i++)
    {
        // Wähle einen zufälligen Index aus der verfügbaren Liste
        int randomIndex = Random.Range(0, availableSpawnPoints.Count);

        // Spawne das FarmerPrefab an der Position und Rotation des ausgewählten Spawnpunkts
        Instantiate(FarmerPrefab, availableSpawnPoints[randomIndex].transform.position, availableSpawnPoints[randomIndex].transform.rotation);

        // Entferne den verwendeten Spawnpunkt aus der Liste
        availableSpawnPoints.RemoveAt(randomIndex);
    }

    Debug.Log($"{numberOfPrefabsToSpawn} Farmer wurden zufällig an einzigartigen Positionen gespawnt.");
    }

}
