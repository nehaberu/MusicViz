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
        currentScale = Mathf.Lerp(currentScale, targetScale, Time.deltaTime * 2f);
        mandalaTransform.localScale = Vector3.one * currentScale;
    }

    public void SetScale(float scale) => targetScale = scale;

    public void SetColor(Color color)
    {
        if (mandalaMaterial != null)
            mandalaMaterial.color = color;
    }

    public void SetComplexity(int s, int l) { /* Unused now */ }
}
