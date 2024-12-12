using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageSystem : MonoBehaviour
{
    public GameObject messageBox;
    public GameObject messagePrefab;
    public int maxMessages = 5;
    public float messageDuration = 5f;
    public float delayBeforeFade = 2f; // 2 Sekunden Verzögerung bevor das Verblassen beginnt

    private Queue<GameObject> activeMessages = new Queue<GameObject>();

    public void ShowMessage(string messageText)
    {
        if (messageBox == null || messagePrefab == null)
        {
            Debug.LogError("MessageBox oder MessagePrefab ist nicht gesetzt.");
            return;
        }

        GameObject newMessage = Instantiate(messagePrefab, messageBox.transform);
        TMP_Text messageComponent = newMessage.GetComponent<TMP_Text>();

        if (messageComponent == null)
        {
            Debug.LogError("MessagePrefab enthält keine TMP_Text-Komponente.");
            Destroy(newMessage);
            return;
        }

        messageComponent.text = messageText;
        activeMessages.Enqueue(newMessage);

        if (activeMessages.Count > maxMessages)
        {
            GameObject oldestMessage = activeMessages.Dequeue();
            Destroy(oldestMessage);
        }

        StartCoroutine(FadeOutAndRemoveMessage(newMessage, messageDuration, delayBeforeFade));
    }

    private IEnumerator FadeOutAndRemoveMessage(GameObject message, float duration, float delayBeforeFade)
    {
        TMP_Text messageText = message.GetComponent<TMP_Text>();
        if (messageText == null) yield break;

        Color originalColor = messageText.color;
        float elapsedTime = 0f;

        // Wartezeit von 2 Sekunden bevor das Verblassen beginnt
        yield return new WaitForSeconds(delayBeforeFade);

        // Beginne mit dem Verblassen nach der Verzögerung
        while (elapsedTime < (duration - delayBeforeFade))
        {
            elapsedTime += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / (duration - delayBeforeFade)); // Fade beginnt nach 2 Sekunden
            messageText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        if (activeMessages.Contains(message))
        {
            activeMessages.Dequeue();
        }

        Destroy(message);
    }
}
