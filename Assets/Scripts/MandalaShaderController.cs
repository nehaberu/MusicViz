using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MandalaShaderController : MonoBehaviour
{
    [Header("Material Settings")]
    [SerializeField] private Material mandalaMaterial;
    
    [Header("Color Animation")]
    [SerializeField] private Color baseColor = Color.blue;
    [SerializeField] private Color accentColor = Color.cyan;
    [SerializeField] private float colorTransitionSpeed = 1.0f; 
    
    [Header("Pattern Settings")]
    [SerializeField] [Range(0, 5)] private float patternIntensity = 1.0f;
    [SerializeField] [Range(0, 10)] private float patternScale = 3.0f;
    [SerializeField] [Range(0, 5)] private float patternSpeed = 1.0f;
    [SerializeField] [Range(0, 1)] private float glowIntensity = 0.5f;
    
    // Shader property IDs for efficiency
    private int baseColorID;
    private int accentColorID;
    private int patternIntensityID;
    private int patternScaleID;
    private int patternSpeedID;
    private int glowIntensityID;
    private int timeID;
    
    private MeshRenderer meshRenderer;
    private float animationTime = 0f;
    
    private void Awake()
    {
        // Cache shader property IDs
        baseColorID = Shader.PropertyToID("_BaseColor");
        accentColorID = Shader.PropertyToID("_AccentColor");
        patternIntensityID = Shader.PropertyToID("_PatternIntensity");
        patternScaleID = Shader.PropertyToID("_PatternScale");
        patternSpeedID = Shader.PropertyToID("_PatternSpeed");
        glowIntensityID = Shader.PropertyToID("_GlowIntensity");
        timeID = Shader.PropertyToID("_Time");
        
        // Get components
        meshRenderer = GetComponent<MeshRenderer>();
        
        // Initialize material if needed
        if (mandalaMaterial == null && meshRenderer != null)
        {
            mandalaMaterial = meshRenderer.material;
        }
        
        // Update the material with initial values
        UpdateMaterialProperties();
    }
    
    private void Update()
    {
        animationTime += Time.deltaTime;
        
        // Update time property in shader
        if (mandalaMaterial != null)
        {
            mandalaMaterial.SetFloat(timeID, animationTime);
        }
    }
    
    private void UpdateMaterialProperties()
    {
        if (mandalaMaterial == null) return;
        
        // Update all material properties
        mandalaMaterial.SetColor(baseColorID, baseColor);
        mandalaMaterial.SetColor(accentColorID, accentColor);
        mandalaMaterial.SetFloat(patternIntensityID, patternIntensity);
        mandalaMaterial.SetFloat(patternScaleID, patternScale);
        mandalaMaterial.SetFloat(patternSpeedID, patternSpeed);
        mandalaMaterial.SetFloat(glowIntensityID, glowIntensity);
    }
    
    // Public methods to be called by the visualization manager
    
    public void SetBaseColor(Color color)
    {
        baseColor = color;
        if (mandalaMaterial != null)
        {
            mandalaMaterial.SetColor(baseColorID, baseColor);
        }
    }
    
    public void SetAccentColor(Color color)
    {
        accentColor = color;
        if (mandalaMaterial != null)
        {
            mandalaMaterial.SetColor(accentColorID, accentColor);
        }
    }
    
    public void SetPatternIntensity(float intensity)
    {
        patternIntensity = Mathf.Clamp(intensity, 0f, 5f);
        if (mandalaMaterial != null)
        {
            mandalaMaterial.SetFloat(patternIntensityID, patternIntensity);
        }
    }
    
    public void SetPatternScale(float scale)
    {
        patternScale = Mathf.Clamp(scale, 0f, 10f);
        if (mandalaMaterial != null)
        {
            mandalaMaterial.SetFloat(patternScaleID, patternScale);
        }
    }
    
    public void SetPatternSpeed(float speed)
    {
        patternSpeed = Mathf.Clamp(speed, 0f, 5f);
        if (mandalaMaterial != null)
        {
            mandalaMaterial.SetFloat(patternSpeedID, patternSpeed);
        }
    }
    
    public void SetGlowIntensity(float intensity)
    {
        glowIntensity = Mathf.Clamp01(intensity);
        if (mandalaMaterial != null)
        {
            mandalaMaterial.SetFloat(glowIntensityID, glowIntensity);
        }
    }
    
    // Preset configurations for different phases
    public void ApplyEmergencePreset()
    {
        SetBaseColor(new Color(0.2f, 0.4f, 0.8f));
        SetAccentColor(new Color(0.3f, 0.5f, 0.9f, 0.7f));
        SetPatternIntensity(0.5f);
        SetPatternScale(1.5f);
        SetPatternSpeed(0.5f);
        SetGlowIntensity(0.2f);
    }
    
    public void ApplyCuriosityPreset()
    {
        SetBaseColor(new Color(0.2f, 0.7f, 0.8f));
        SetAccentColor(new Color(0.3f, 0.8f, 0.7f, 0.8f));
        SetPatternIntensity(1.0f);
        SetPatternScale(2.0f);
        SetPatternSpeed(0.8f);
        SetGlowIntensity(0.4f);
    }
    
    public void ApplyBuildupPreset()
    {
        SetBaseColor(new Color(0.4f, 0.6f, 0.2f));
        SetAccentColor(new Color(0.7f, 0.6f, 0.2f, 0.9f));
        SetPatternIntensity(2.0f);
        SetPatternScale(3.0f);
        SetPatternSpeed(1.2f);
        SetGlowIntensity(0.6f);
    }
    
    public void ApplyPeakPreset()
    {
        SetBaseColor(new Color(0.9f, 0.4f, 0.1f));
        SetAccentColor(new Color(1.0f, 0.7f, 0.0f, 1.0f));
        SetPatternIntensity(4.0f);
        SetPatternScale(4.0f);
        SetPatternSpeed(2.0f);
        SetGlowIntensity(1.0f);
    }
    
    public void ApplyDescentPreset()
    {
        SetBaseColor(new Color(0.6f, 0.4f, 0.8f));
        SetAccentColor(new Color(0.5f, 0.3f, 0.9f, 0.9f));
        SetPatternIntensity(2.0f);
        SetPatternScale(3.0f);
        SetPatternSpeed(1.0f);
        SetGlowIntensity(0.7f);
    }
    
    public void ApplyResolutionPreset()
    {
        SetBaseColor(new Color(0.9f, 0.9f, 1.0f));
        SetAccentColor(new Color(1.0f, 1.0f, 1.0f, 0.8f));
        SetPatternIntensity(0.7f);
        SetPatternScale(1.0f);
        SetPatternSpeed(0.3f);
        SetGlowIntensity(0.5f);
    }
}