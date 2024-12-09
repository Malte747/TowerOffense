using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBibleScript : MonoBehaviour
{
   public static Dictionary<Vector3, GameObject> EnemyBible = new Dictionary<Vector3,GameObject>();

    void Update()
    {
        EnemyBible.Clear();
    }
}
