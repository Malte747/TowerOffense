using UnityEngine;

public class MuteMixer : MonoBehaviour
{
    public AudioManager crossfadeAudio; // Referenz zum CrossfadeAudio-Skript

    void Update()
    {
        // Beispiel: Drücke "1", um Track mit Index 1 abzuspielen
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (crossfadeAudio != null)
            {
                //crossfadeAudio.CrossfadeToClip(1);
            }
            else
            {
                Debug.LogWarning("CrossfadeAudio-Skript ist nicht zugewiesen!");
            }
        }
    }
}
