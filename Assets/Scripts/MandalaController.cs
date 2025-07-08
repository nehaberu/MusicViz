using UnityEngine;

public class MandalaController : MonoBehaviour
{
    [SerializeField] private GameObject mandalaObject;
    [SerializeField] private float initialScale = 1.0f;
    [SerializeField] private Material mandalaMaterial;
    [SerializeField] private Color initialColor = Color.white;

    private Transform mandalaTransform;
    private float currentScale;
    private float targetScale;

    private float rotationSpeed = 10f;
    private float targetRotationSpeed = 10f;

    private Color currentColor;
    private Color targetColor;

    private Color currentEmission;
    private Color targetEmission;

    [Header("Transition Speeds")]
    [SerializeField] private float scaleLerpSpeed = 2f;
    [SerializeField] private float rotationLerpSpeed = 5f;
    [SerializeField] private float colorLerpSpeed = 2f;

    void Start()
    {
        Debug.Log("✅ Unity IS logging");

        if (mandalaObject == null)
            mandalaObject = this.gameObject;

        mandalaTransform = mandalaObject.transform;
        currentScale = targetScale = initialScale;

        if (mandalaMaterial != null)
        {
            currentColor = targetColor = initialColor;
            currentEmission = targetEmission = initialColor * 2f;

            mandalaMaterial.color = currentColor;
            mandalaMaterial.SetColor("_EmissionColor", currentEmission);
        }
    }

    void Update()
    {
        if (mandalaTransform == null || mandalaMaterial == null) return;

        // 🌱 Smooth scale change
        currentScale = Mathf.Lerp(currentScale, targetScale, Time.deltaTime * scaleLerpSpeed);
        mandalaTransform.localScale = Vector3.one * currentScale;

        // 🌀 Smooth rotation speed change
        rotationSpeed = Mathf.Lerp(rotationSpeed, targetRotationSpeed, Time.deltaTime * rotationLerpSpeed);
        mandalaTransform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        // 🎨 Smooth color transition
        currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * colorLerpSpeed);
        mandalaMaterial.color = currentColor;

        // 🌟 Smooth emission glow
        currentEmission = Color.Lerp(currentEmission, targetEmission, Time.deltaTime * colorLerpSpeed);
        mandalaMaterial.SetColor("_EmissionColor", currentEmission);
    }

    // External control for scale
    public void SetScale(float scale)
    {
        targetScale = scale;
    }

    // Set base color and glow smoothly
    public void SetColor(Color color)
    {
        targetColor = color;
        targetEmission = color * 2f;
    }

    public void SetEmission(Color emission)
    {
        targetEmission = emission;
    }

    public void SetRotationSpeed(float speedFactor)
    {
        float minSpeed = 5f;
        float maxSpeed = 80f;
        targetRotationSpeed = Mathf.Lerp(minSpeed, maxSpeed, speedFactor);
    }

    public void SetComplexity(int s, int l) { /* Not used yet */ }
}
