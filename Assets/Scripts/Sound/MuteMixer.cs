using UnityEngine;

public class MuteMixer : MonoBehaviour
{
    AudioManager audioManager; // Referenz zum CrossfadeAudio-Skript

    void Update()
    {
        // Beispiel: Drücke "1", um Track mit Index 1 abzuspielen
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (audioManager != null)
            {
                audioManager.CrossfadeToClip(1);
            }
            else
            {
                Debug.LogWarning("CrossfadeAudio-Skript ist nicht zugewiesen!");
            }
        }
    }
}
