using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cam;

    void Start()
    {
        GameObject cameraObject = GameObject.Find("Camera");
        
        cam = cameraObject.transform; 
    }

    void LateUpdate()
    {
        if (cam != null)
        {
            transform.LookAt(cam.transform.position);
        }
    }
}

