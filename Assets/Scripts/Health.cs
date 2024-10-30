using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
