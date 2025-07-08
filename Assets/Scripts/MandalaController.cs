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

    void Start()
    {
        Debug.Log("✅ Unity IS logging");

        if (mandalaObject == null)
            mandalaObject = this.gameObject;

        mandalaTransform = mandalaObject.transform;
        currentScale = targetScale = initialScale;

        if (mandalaMaterial != null)
            mandalaMaterial.color = initialColor;
    }

    void Update()
    {
        if (mandalaTransform == null) return;

        // 🌱 Smooth scale change
        currentScale = Mathf.Lerp(currentScale, targetScale, Time.deltaTime * 2f);
        mandalaTransform.localScale = Vector3.one * currentScale;

        // 🌀 Smooth rotation speed change
        rotationSpeed = Mathf.Lerp(rotationSpeed, targetRotationSpeed, Time.deltaTime * 5f);
        mandalaTransform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    // 🎨 Called from RealTimeMoodAnalyzer to control scale
    public void SetScale(float scale)
    {
        targetScale = scale;
    }

    // 🌈 Called to change color & emission glow
    public void SetColor(Color color)
    {
        if (mandalaMaterial != null)
        {
            mandalaMaterial.color = color;
            Color emission = color * 2f;
            mandalaMaterial.SetColor("_EmissionColor", emission);
        }
    }

    // 🌀 Now responsive to mood intensity!
    public void SetRotationSpeed(float speedFactor)
    {
        float minSpeed = 5f;
        float maxSpeed = 80f;
        targetRotationSpeed = Mathf.Lerp(minSpeed, maxSpeed, speedFactor);
    }

    public void SetComplexity(int s, int l) { /* Not used yet */ }

    public void SetEmission(Color emission)
    {
        if (mandalaMaterial != null)
            mandalaMaterial.SetColor("_EmissionColor", emission);
    }
}
