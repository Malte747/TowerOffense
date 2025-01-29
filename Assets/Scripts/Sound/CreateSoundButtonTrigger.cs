using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreateSoundButtonTrigger : MonoBehaviour
{

    public Button button;
    public AudioManager AudioManager;

    // Start is called before the first frame update
    void Start()
    {

        AudioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        button.onClick.AddListener(() =>
        {
            AudioManager.PlayUISound(0); // Call AudioManager.PlayUISound
        });

        EventTrigger trigger = button.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = button.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) =>
        {
            AudioManager.PlayUISound(1); // Call AudioManager.PlayUISound
        });
        trigger.triggers.Add(entry);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
