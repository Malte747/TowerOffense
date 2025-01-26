using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderSound : MonoBehaviour
{
    public Slider slider;
    public AudioManager AudioManager;
    private bool isPlaying = false;

    void Start()
    {
        // Add EventTrigger for Drag
        EventTrigger trigger = slider.GetComponent<EventTrigger>();

        AudioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        if (slider != null)
        {

            if (trigger == null)
            {
                trigger = slider.gameObject.AddComponent<EventTrigger>();
            }

            EventTrigger.Entry dragEntry = new EventTrigger.Entry();
            dragEntry.eventID = EventTriggerType.Drag;
            dragEntry.callback.AddListener((data) =>
            {
                StartCoroutine(SliderSoundSFX());
            });
            trigger.triggers.Add(dragEntry);
        }
    }


    IEnumerator SliderSoundSFX()
    {

        if(isPlaying == false)
        {
            isPlaying = true;
            AudioManager.PlayUISound(0);
            yield return new WaitForSecondsRealtime(.2f);
            isPlaying = false;
        }
    }

}

