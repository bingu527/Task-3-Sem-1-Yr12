using UnityEngine;


public class ChemicalReaction : MonoBehaviour
{
    public Renderer liquidRenderer;
    public string topColorPropertyName = "_top_color";
    public string sideColorPropertyName = "_side_color";
    public Color reactionTopColor = Color.white;
    public Color reactionSideColor = Color.white;
    public float colorChangeDuration = 5f;
    public GameObject smokeEffectPrefab;
    public Transform smokeSpawnPoint;

    private Color originalTopColor;
    private Color originalSideColor;
    private Material liquidMaterial;
    private bool reactionActive = false;
    private float reactionTimer = 0f;
    private bool smokePlayed = false;

    private void Start()
    {
        if (liquidRenderer != null)
        {
            liquidMaterial = Instantiate(liquidRenderer.material); // Create an instance
            liquidRenderer.material = liquidMaterial; // Assign the instance back to the renderer
            if (liquidMaterial.HasProperty(topColorPropertyName))
            {
                originalTopColor = liquidMaterial.GetColor(topColorPropertyName);
            }
            else
            {
                Debug.LogWarning($"Material does not have a color property named {topColorPropertyName}");
                originalTopColor = Color.white;
            }
            if (liquidMaterial.HasProperty(sideColorPropertyName))
            {
                originalSideColor = liquidMaterial.GetColor(sideColorPropertyName);
            }
            else
            {
                Debug.LogWarning($"Material does not have a color property named {sideColorPropertyName}");
                originalSideColor = Color.white;
            }
        }
        else
        {
            Debug.LogWarning("Liquid Renderer not assigned!");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (IsPouringLiquid(other))
        {
            StartReaction();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsPouringLiquid(other) && reactionActive)
        {
            reactionActive = true;
            reactionTimer += Time.deltaTime;
            UpdateColors();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsPouringLiquid(other))
        {
            StopReaction();
        }
    }

    private bool IsPouringLiquid(Collider other)
    {
        return other.CompareTag("reactionpoint");
    }

    private void StartReaction()
    {
        if (!reactionActive)
        {
            reactionActive = true;
            reactionTimer = 0f;
        }
    }
    private void StopReaction()
    {
        reactionActive = false;
        reactionTimer = 0f;
    }

    private void UpdateColors()
    {
        if (liquidMaterial != null)
        {
            float t = Mathf.Clamp01(reactionTimer / colorChangeDuration);
            Color currentTopColor = Color.Lerp(originalTopColor, reactionTopColor, t);
            Color currentSideColor = Color.Lerp(originalSideColor, reactionSideColor, t);
            liquidMaterial.SetColor(topColorPropertyName, currentTopColor);
            liquidMaterial.SetColor(sideColorPropertyName, currentSideColor);

            if (t >= 1f && !smokePlayed)
            {
                PlaySmokeEffect();
                smokePlayed = true;
            }
        }
    }
    private void PlaySmokeEffect()
    {
        if (smokeEffectPrefab != null)
        {
            Vector3 spawnPosition = smokeSpawnPoint != null ? smokeSpawnPoint.position : transform.position + Vector3.up * 0.5f;
            Instantiate(smokeEffectPrefab, spawnPosition, Quaternion.identity);
        }
    }
    public void ResetReaction()
    {
        reactionActive = false;
        reactionTimer = 0f;
        smokePlayed = false;

        if (liquidMaterial != null)
        {
            liquidMaterial.SetColor(topColorPropertyName, originalTopColor);
            liquidMaterial.SetColor(sideColorPropertyName, originalSideColor);
        }
    }
}