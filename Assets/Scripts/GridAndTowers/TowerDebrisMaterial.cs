using UnityEngine;
using System.Collections;

public class DuplicateMaterialForChildren : MonoBehaviour
{
    [SerializeField] private Material originalMaterial;                // To store the original material (for the root object)
    private Material temporaryMaterial;               // Temporary duplicated material
    private Renderer[] renderers;                     // Array to store all Renderer components in children

    public float fadeDuration = 6.0f;
    private float fadeTimer = -2f;

    void Start()
    {
        // Get all Renderers in the GameObject and its children
        renderers = GetComponentsInChildren<Renderer>();

        // If there are renderers, proceed
        if (renderers.Length > 0)
        {
            // Duplicate the first renderer's material as a template
            temporaryMaterial = new Material(originalMaterial);

            temporaryMaterial.SetFloat("_Surface", 1f);

            // Apply the temporary material to all Renderers
            foreach (Renderer renderer in renderers)
            {
                renderer.material = temporaryMaterial;
            }
        }
    }

    void Update()
    {
        fadeTimer += Time.deltaTime;
        if (fadeTimer < fadeDuration && fadeTimer < fadeDuration -1f)
        {
            // Calculate the new alpha value
            float alpha = Mathf.Lerp(1, 0, fadeTimer / fadeDuration);

            // Set the new color with updated alpha
            Color color = temporaryMaterial.color;
            color.a = alpha;
            temporaryMaterial.color = color;
        }
        else //if (fadeTimer > fadeDuration)
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        // Clean up the temporary material to avoid memory leaks
        if (temporaryMaterial != null)
        {
            Destroy(temporaryMaterial);
        }
    }
}
