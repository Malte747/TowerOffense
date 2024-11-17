using UnityEngine;

public class MainTower : MonoBehaviour
{
    float size = 14;
    // Start is called before the first frame update
    void Start()
    {
        TowerKnowsWhereItIs towerScript = GetComponent<TowerKnowsWhereItIs>();

        for (int i = 0; i <= Mathf.Abs(size); i++)
        {
            TowerGridPlacement.TowerBible.Add(new Vector3Int(i, 0, 27), gameObject);

            towerScript.MyCells.Add(new Vector3Int(i, 0, 27));
        }

    }

}
