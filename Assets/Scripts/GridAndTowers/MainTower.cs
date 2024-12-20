using UnityEngine;

public class MainTower : MonoBehaviour
{
    [SerializeField] TowerStats TowerStats;

    float size = 14;
    // Start is called before the first frame update
    void Start()
    {
        Transform rangeIndicator = gameObject.transform.GetChild(1);
        float rangeNumber = TowerStats.attackRange / 20f;
        rangeIndicator.localScale = new Vector3(1, 0.01f, rangeNumber);
        rangeIndicator.localPosition = new Vector3(0, -0.4f, -(rangeNumber / 2f));

        TowerKnowsWhereItIs towerScript = GetComponent<TowerKnowsWhereItIs>();

        for (int i = 0; i <= Mathf.Abs(size); i++)
        {
            TowerGridPlacement.TowerBible.Add(new Vector3Int(i, 0, 27), gameObject);

            towerScript.MyCells.Add(new Vector3Int(i, 0, 27));
        }

    }

}
