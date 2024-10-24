using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class NavMeshBaking : MonoBehaviour
{
    NavMeshSurface surface;

    float bakingProgress;

    
    private void Awake()
    {
        surface = GetComponent<NavMeshSurface>();

        bakingProgress = 0f;
        StartCoroutine(BakeNavMeshAsync());
    }

    private IEnumerator BakeNavMeshAsync()
    {
        Debug.Log("Started Baking NavMesh");
        AsyncOperation BakingAsync = surface.UpdateNavMesh(surface.navMeshData);
        while (!BakingAsync.isDone)
        {
            bakingProgress = BakingAsync.progress * 100;
            Debug.Log($"Progress: {bakingProgress}%");
            yield return null;
        }
        Debug.Log("NavMesh baking completed!");
        bakingProgress = 100f;
    }
}
