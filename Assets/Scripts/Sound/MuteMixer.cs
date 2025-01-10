using UnityEngine;

public class MuteMixer : MonoBehaviour
{
    public AudioManager uiSFX; // Referenz zum AudioManager Script 


    private void Start()
    {
        uiSFX = GameObject.Find("AudioManager").GetComponent<AudioManager>(); //Nimmt den Audio Manager das Script
    }


    void Update()
    {

        //An der stelle, an welcher SFX ausgel�st werden sollen platzieren
        uiSFX.PlayUISound(Random.Range( 0, 1)); //Spielt die gew�nschte SFX Nummer

    }
}
