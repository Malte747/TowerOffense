using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shhhhhh : MonoBehaviour
{
    public GameObject bigRedButton;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            bigRedButton.SetActive(true);
        }
    }
}
