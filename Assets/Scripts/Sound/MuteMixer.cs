using UnityEngine;
using UnityEngine.Audio;

public class MuteMixer : MonoBehaviour
{
    public AudioMixer audioMixer; // Der AudioMixer, der gesteuert werden soll
    public string volumeParameter = "MasterVolume"; // Der Name des Lautstärkeparameters im Mixer
    private bool isMuted = false; // Zustand des Mutes

    // Wird vom Button aufgerufen
    public void ToggleMute()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            // Lautstärke auf den niedrigsten Wert setzen
            audioMixer.SetFloat(volumeParameter, -80f); // -80dB ist praktisch stumm
        }
        else
        {
            // Lautstärke auf 0dB zurücksetzen
            audioMixer.SetFloat(volumeParameter, 0f);
        }
    }
}
