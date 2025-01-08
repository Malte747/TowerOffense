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
        public float spawnChance;
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


    void Start()
    {
        uiManager = GameObject.Find("UiManager").GetComponent<UIManager>();
        sequentialActivator = GameObject.Find("UiManager").GetComponent<SequentialActivator>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    // Aufrufen der Karten
    public void CallCards()
    {
        if(sequentialActivator.upgradePrice > gameManager.attackerGold)
        {
            return; 
        }

        sequentialActivator.BuyUpgradeAttack();
        uiManager.StartCardSelector();
        DeleteCards(); // Lösche vorherige Karten

        List<Card> validCards = cards.FindAll(card => card.tier <= currentTier);

        if (validCards.Count < spawnPositions.Length)
        {
            Debug.LogError("Nicht genügend Karten verfügbar, um alle Positionen zu füllen.");
            return;
        }

        ShuffleList(validCards);

        for (int i = 0; i < spawnPositions.Length; i++)
        {
            GameObject cardPrefab = validCards[i].prefab;
            GameObject spawnedCard = Instantiate(cardPrefab, spawnPositions[i], Quaternion.identity, canvasTransform);
            spawnedCards.Add(spawnedCard);
        }
    }

    // Zufälliges Mischen der Kartenliste
    private void ShuffleList(List<Card> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Card temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    // Lösche alle derzeitigen Karten
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
        Debug.Log("Alle Karten wurden zurückgesetzt.");



    }
}