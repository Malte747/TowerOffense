using UnityEngine;
using UnityEngine.Audio;

public class MuteMixer : MonoBehaviour
{
    public AudioMixer audioMixer; // Der AudioMixer, der gesteuert werden soll
    public string volumeParameter = "MasterVolume"; // Der Name des Lautst�rkeparameters im Mixer
    private bool isMuted = false; // Zustand des Mutes

    // Wird vom Button aufgerufen
    public void ToggleMute()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            // Lautst�rke auf den niedrigsten Wert setzen
            audioMixer.SetFloat(volumeParameter, -80f); // -80dB ist praktisch stumm
        }
        else
        {
            // Lautst�rke auf 0dB zur�cksetzen
            audioMixer.SetFloat(volumeParameter, 0f);
        }
    }
}
