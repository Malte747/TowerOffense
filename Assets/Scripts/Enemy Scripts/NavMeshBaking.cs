using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;

//  SCRIPT SUMMARY: bakes/updates all NavMeshes needed this round
//      (functional)    Which enemies exist in the scene?
//      (done)          Which meshes are needed this round?
//      (done)          Update meshes async
//      (wip)   Track the baking progress

public class NavMeshBaking : MonoBehaviour
{
    public static bool firstEnemyExists, secondEnemyExists, thirdEnemyExists;
    public static float NavMeshbakingProgress;
    [SerializeField] NavMeshSurface[] surfaces;

    private void Start()
    {
        UpdateNavMeshes();
    }
    public void UpdateNavMeshes()
    {
        Debug.Log("Started Baking NavMesh");

        NavMeshbakingProgress = 0f;
        if (firstEnemyExists) StartCoroutine(BakeNavMesh(surfaces[0]));
        if (secondEnemyExists) StartCoroutine(BakeNavMesh(surfaces[1]));
        if (thirdEnemyExists) StartCoroutine(BakeNavMesh(surfaces[2]));

    }

    private IEnumerator BakeNavMesh(NavMeshSurface surface)
    {
        float progress = 0f;
        
        AsyncOperation BakingAsync = surface.UpdateNavMesh(surface.navMeshData);
        while (!BakingAsync.isDone)
        {
            progress = BakingAsync.progress * 100;
            yield return null;
        }
        progress = 100f;
    }

    private void Update()
    {
        
    }
}
