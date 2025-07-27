using System.Collections;
using UnityEngine;

public class MusicVisualizationManager : MonoBehaviour
{
    // Controls the progression of the mandala visualization based on music playback.
// It switches between phases at set times, and applies corresponding visual effects, mandala sprites, and shader presets.

    [Header("Audio Settings")]
    [SerializeField] private AudioClip musicTrack;
    [SerializeField] private AudioSource audioSource;

    [Header("Mandala References")]
    [SerializeField] private MandalaController mandalaController;
    [SerializeField] private MandalaImageController imageController;
    [SerializeField] private MandalaShaderController shaderController;

    [Header("Visualization Section Markers")]
    [SerializeField] private float emergenceStartTime = 0f;
    [SerializeField] private float curiosityStartTime = 30f;
    [SerializeField] private float buildupStartTime = 45f;
    [SerializeField] private float peakStartTime = 60f;
    [SerializeField] private float descentStartTime = 80f;
    [SerializeField] private float resolutionStartTime = 100f;

    private enum MandalaPhase { Emergence, Curiosity, Buildup, Peak, Descent, Resolution }
    private MandalaPhase currentPhase = MandalaPhase.Emergence;
    private MandalaPhase previousPhase = MandalaPhase.Resolution;

    private bool isPlaying = false;
    private Coroutine currentEffectRoutine;

    void Start()
    {
        // Initialize audio source and play music
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource != null && musicTrack != null)
        {
            audioSource.clip = musicTrack;
            audioSource.Play();
        }

        isPlaying = true;
        ApplyVisualEffects(); // Trigger initial visuals
    }

    void Update()
    {
        if (!isPlaying || audioSource == null || !audioSource.isPlaying)
            return;

        float currentTime = audioSource.time;
        UpdateCurrentPhase(currentTime);

        // Apply visuals only if phase has changed
        if (currentPhase != previousPhase)
        {
            ApplyVisualEffects();
            previousPhase = currentPhase;
        }
    }

    private void UpdateCurrentPhase(float currentTime)
    {
        // Determine current phase based on time
        if (currentTime >= resolutionStartTime)
            currentPhase = MandalaPhase.Resolution;
        else if (currentTime >= descentStartTime)
            currentPhase = MandalaPhase.Descent;
        else if (currentTime >= peakStartTime)
            currentPhase = MandalaPhase.Peak;
        else if (currentTime >= buildupStartTime)
            currentPhase = MandalaPhase.Buildup;
        else if (currentTime >= curiosityStartTime)
            currentPhase = MandalaPhase.Curiosity;
        else
            currentPhase = MandalaPhase.Emergence;
    }

    private void ApplyVisualEffects()
    {
        // Update image sprite for the current phase
        if (imageController != null)
        {
            string phaseName = currentPhase.ToString().ToLower();
            Sprite spriteToUse = GetPhaseSprite(phaseName);
            imageController.SetPhaseSmooth(spriteToUse, 2f);
        }

        // Apply shader preset based on current phase
        if (shaderController != null)
        {
            switch (currentPhase)
            {
                case MandalaPhase.Emergence: shaderController.ApplyEmergencePreset(); break;
                case MandalaPhase.Curiosity: shaderController.ApplyCuriosityPreset(); break;
                case MandalaPhase.Buildup: shaderController.ApplyBuildupPreset(); break;
                case MandalaPhase.Peak: shaderController.ApplyPeakPreset(); break;
                case MandalaPhase.Descent: shaderController.ApplyDescentPreset(); break;
                case MandalaPhase.Resolution: shaderController.ApplyResolutionPreset(); break;
            }
        }

        // Stop previous animation coroutine if running
        if (currentEffectRoutine != null)
            StopCoroutine(currentEffectRoutine);

        float duration = TimeUntilNextPhase();

        // Animate based on phase type
        switch (currentPhase)
        {
            case MandalaPhase.Emergence:
            case MandalaPhase.Curiosity:
            case MandalaPhase.Buildup:
            case MandalaPhase.Peak:
                currentEffectRoutine = StartCoroutine(AnimateScaleFade(0.2f, 1.0f, 0f, 1f, duration));
                break;

            case MandalaPhase.Descent:
            case MandalaPhase.Resolution:
                currentEffectRoutine = StartCoroutine(AnimateScaleFade(1.0f, 0.2f, 1f, 0f, duration));
                break;
        }
    }

    private float TimeUntilNextPhase()
    {
        float currentTime = audioSource.time;

        if (currentTime < curiosityStartTime) return curiosityStartTime - currentTime;
        if (currentTime < buildupStartTime) return buildupStartTime - currentTime;
        if (currentTime < peakStartTime) return peakStartTime - currentTime;
        if (currentTime < descentStartTime) return descentStartTime - currentTime;
        if (currentTime < resolutionStartTime) return resolutionStartTime - currentTime;

        return 20f; // default fallback duration
    }

    private Sprite GetPhaseSprite(string phase)
    {
        // Return sprite matching the given phase name
        switch (phase.ToLower())
        {
            case "emergence": return imageController.emergence;
            case "curiosity": return imageController.curiosity;
            case "buildup": return imageController.buildup;
            case "peak": return imageController.peak;
            case "descent": return imageController.descent;
            case "resolution": return imageController.resolution;
            default: return null;
        }
    }

    private IEnumerator AnimateScaleFade(float startScale, float endScale, float startAlpha, float endAlpha, float duration)
    {
        float time = 0f;
        imageController?.SetRotationSpeed(8f); // slow rotation during transition

        while (time < duration)
        {
            float t = time / duration;

            float scale = Mathf.Lerp(startScale, endScale, t);
            float alpha = Mathf.Lerp(startAlpha, endAlpha, t);

            mandalaController?.SetScale(scale);
            imageController?.SetAlpha(alpha);

            time += Time.deltaTime;
            yield return null;
        }

        // Final values
        mandalaController?.SetScale(endScale);
        imageController?.SetAlpha(endAlpha);
    }
}
