using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseIndicator;

    [SerializeField]
    private GridMouseInput gridMouseInput;
    private void Update()
    {
        Vector3 mousePos = gridMouseInput.GetSelectedMapPos();
        mouseIndicator.transform.position = mousePos;
    }
}
