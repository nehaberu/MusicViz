using UnityEngine;

// Controls mandala's scale, rotation, and color based on mood input
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

        // Scale animation
        currentScale = Mathf.Lerp(currentScale, targetScale, Time.deltaTime * scaleLerpSpeed);
        mandalaTransform.localScale = Vector3.one * currentScale;

        // Rotation animation
        rotationSpeed = Mathf.Lerp(rotationSpeed, targetRotationSpeed, Time.deltaTime * rotationLerpSpeed);
        mandalaTransform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        // Base color transition
        currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * colorLerpSpeed);
        mandalaMaterial.color = currentColor;

        // Emission glow transition
        currentEmission = Color.Lerp(currentEmission, targetEmission, Time.deltaTime * colorLerpSpeed);
        mandalaMaterial.SetColor("_EmissionColor", currentEmission);
    }

    public void SetScale(float scale)
    {
        targetScale = scale;
    }

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

}
