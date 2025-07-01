using System.Collections;
using UnityEngine;

public class MusicVisualizationManager : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip musicTrack;
    [SerializeField] private AudioSource audioSource;

    [Header("Mandala References")]
    [SerializeField] private MandalaController mandalaController;
    [SerializeField] private MandalaImageController imageController;

    [Header("Visualization Section Markers")]
    [SerializeField] private float emergenceStartTime = 0f;
    [SerializeField] private float curiosityStartTime = 45f;
    [SerializeField] private float buildupStartTime = 80f;
    [SerializeField] private float peakStartTime = 130f;
    [SerializeField] private float descentStartTime = 180f;
    [SerializeField] private float resolutionStartTime = 240f;

    private enum MandalaPhase { Emergence, Curiosity, Buildup, Peak, Descent, Resolution }
    private MandalaPhase currentPhase = MandalaPhase.Emergence;
    private MandalaPhase previousPhase = MandalaPhase.Resolution;

    private bool isPlaying = false;
    private Coroutine currentEffectRoutine;

    void Start()
    {
        Debug.Log("🎬 MusicVisualizationManager is running");

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource != null && musicTrack != null)
        {
            audioSource.clip = musicTrack;
            audioSource.Play();
            Debug.Log("🎶 Music playback started");
        }
        else
        {
            Debug.LogWarning("❌ Missing audio source or track!");
        }

        isPlaying = true;
        ApplyVisualEffects();
    }

    void Update()
    {
        if (!isPlaying || audioSource == null || !audioSource.isPlaying)
            return;

        float currentTime = audioSource.time;
        UpdateCurrentPhase(currentTime);

        if (currentPhase != previousPhase)
        {
            Debug.Log($"🔁 Phase changed: {previousPhase} → {currentPhase}");
            ApplyVisualEffects();
            previousPhase = currentPhase;
        }
    }

    private void UpdateCurrentPhase(float currentTime)
    {
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
        Debug.Log("✨ Applying visuals for phase: " + currentPhase);

        if (imageController != null)
        {
            string phaseName = currentPhase.ToString().ToLower();
            Sprite spriteToUse = GetPhaseSprite(phaseName);
            imageController.SetPhaseSmooth(spriteToUse, 2f);
        }

        if (currentEffectRoutine != null)
            StopCoroutine(currentEffectRoutine);

        float duration = TimeUntilNextPhase();

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
        return 20f;
    }

    private Sprite GetPhaseSprite(string phase)
    {
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
        imageController?.SetRotationSpeed(8f);

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

        // Final state
        mandalaController?.SetScale(endScale);
        imageController?.SetAlpha(endAlpha);
    }
}
