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
    [SerializeField] NavMeshSurface surface;

    private void Start()
    {
        surface = GetComponent<NavMeshSurface>();
        StartCoroutine(BakeNavMesh());
    }

    public IEnumerator BakeNavMesh()
    {
        yield return new WaitForSeconds(0.01f);
        float progress = 0f;
        
        AsyncOperation BakingAsync = surface.UpdateNavMesh(surface.navMeshData);
        while (!BakingAsync.isDone)
        {
            progress = BakingAsync.progress * 100;
            yield return null;
        }
        progress = 100f;
        NavMeshbakingProgress = progress;
    }

}
