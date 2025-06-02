using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(AudioSource))]
public class MandalaController : MonoBehaviour
{
    [Header("Mandala Settings")]
    [SerializeField] private GameObject mandalaObject;
    [SerializeField] private int rotationSpeed = 10;
    [SerializeField] private float initialScale = 1.0f;

    [Header("Color Settings")]
    [SerializeField] private Color initialColor = Color.blue;
    [SerializeField] private Material mandalaMaterial;
    [SerializeField] private MandalaShaderController shaderController;

    [Header("Timeline Control")]
    [SerializeField] private PlayableDirector timelineDirector;
    [SerializeField] private AudioSource audioSource;

    // Shape complexity controls
    [Range(3, 24)]
    [SerializeField] private int segments = 8;
    [Range(1, 10)]
    [SerializeField] private int layers = 3;

    // Private variables
    private Transform mandalaTransform;
    private float currentScale;
    private float targetScale;
    private Color targetColor;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        // Try auto-assign mandala object to this GameObject if missing
        if (mandalaObject == null)
            mandalaObject = this.gameObject;

        if (mandalaObject != null)
        {
            mandalaTransform = mandalaObject.transform;
            currentScale = initialScale;
            targetScale = initialScale;

            if (mandalaMaterial != null)
            {
                mandalaMaterial.color = initialColor;
                targetColor = initialColor;
            }
        }
        else
        {
            Debug.LogError("Mandala object not assigned!");
        }
    }

    void Update()
    {
        if (mandalaTransform == null) return;

        mandalaTransform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        currentScale = Mathf.Lerp(currentScale, targetScale, Time.deltaTime * 2.0f);
        mandalaTransform.localScale = new Vector3(currentScale, currentScale, currentScale);
    }

    public void SetScale(float scale)
    {
        targetScale = scale;
    }

    public void SetColor(Color color)
    {
        targetColor = color;

        if (shaderController != null)
        {
            shaderController.SetBaseColor(color);
        }
        else if (mandalaMaterial != null)
        {
            mandalaMaterial.color = color;
        }
    }

    public void SetComplexity(int newSegments, int newLayers)
    {
        segments = Mathf.Clamp(newSegments, 3, 24);
        layers = Mathf.Clamp(newLayers, 1, 10);
        UpdateMandalaShape();
    }

    private void UpdateMandalaShape()
    {
        Debug.Log($"Updating mandala with {segments} segments and {layers} layers");
    }

    public void PlayAudio()
    {
        if (audioSource != null && !audioSource.isPlaying)
            audioSource.Play();
    }

    public void PauseAudio()
    {
        if (audioSource != null && audioSource.isPlaying)
            audioSource.Pause();
    }

    public void StopAudio()
    {
        if (audioSource != null)
            audioSource.Stop();
    }
}
