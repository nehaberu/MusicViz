using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class MusicVisualizationManager : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip musicTrack;
    [SerializeField] private AudioSource audioSource;

    [Header("Timeline Control")]
    [SerializeField] private PlayableDirector timelineDirector;
    [SerializeField] private TimelineAsset timelineAsset;

    [Header("Mandala References")]
    [SerializeField] private MandalaController mandalaController;
    [SerializeField] private MandalaImageController imageController;

    [Header("Visualization Section Markers")]
    [SerializeField] private float emergenceStartTime = 0f;
    [SerializeField] private float curiosityStartTime = 10f;
    [SerializeField] private float buildupStartTime = 20f;
    [SerializeField] private float peakStartTime = 30f;
    [SerializeField] private float descentStartTime = 40f;
    [SerializeField] private float resolutionStartTime = 50f;

    private enum MandalaPhase { Emergence, Curiosity, Buildup, Peak, Descent, Resolution }
    private MandalaPhase currentPhase = MandalaPhase.Emergence;
    private MandalaPhase previousPhase = MandalaPhase.Resolution; // ensures first visual is triggered

    private bool isPlaying = false;

    void Start()
    {
        Debug.Log("🎬 MusicVisualizationManager is running");

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource != null && musicTrack != null)
        {
            audioSource.clip = musicTrack;
            audioSource.Play();
            Debug.Log("🎶 Forcing music playback manually");
        }
        else
        {
            Debug.LogWarning("❌ AudioSource or MusicTrack missing");
        }

        isPlaying = true;
        timelineDirector?.Play();

        ApplyVisualEffects(); // initial visual
    }

    void Update()
    {
        if (!isPlaying || audioSource == null)
        {
            Debug.LogWarning("❌ Update skipped - Not playing or no audioSource");
            return;
        }

        if (!audioSource.isPlaying)
        {
            Debug.LogWarning("⛔ Audio is not playing — timeline paused?");
            return;
        }

        float currentTime = audioSource.time;
        Debug.Log($"🎵 Music Time: {currentTime:F2}");

        UpdateCurrentPhase(currentTime);
        Debug.Log($"🧠 Current phase = {currentPhase}");

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
            Debug.Log("🖼 Changing sprite to: " + phaseName);
            imageController.SetPhase(phaseName);
        }

        switch (currentPhase)
        {
            case MandalaPhase.Emergence:
                mandalaController?.SetColor(new Color(0.2f, 0.4f, 0.8f));
                StartCoroutine(AnimateEmergenceFadeIn(2f));
                StartCoroutine(AnimateEmergenceScale(2f));
                break;

            case MandalaPhase.Curiosity:
                mandalaController?.SetColor(new Color(0.2f, 0.7f, 0.8f));
                mandalaController?.SetScale(1.0f);
                imageController?.SetRotationSpeed(20f);
                break;

            case MandalaPhase.Buildup:
                mandalaController?.SetColor(new Color(0.4f, 0.6f, 0.2f, 0.9f));
                mandalaController?.SetScale(1.2f);
                imageController?.SetRotationSpeed(30f);
                break;

            case MandalaPhase.Peak:
                mandalaController?.SetColor(new Color(0.9f, 0.4f, 0.1f));
                mandalaController?.SetScale(1.5f);
                imageController?.SetRotationSpeed(40f);
                break;

            case MandalaPhase.Descent:
                mandalaController?.SetColor(new Color(0.6f, 0.4f, 0.8f));
                mandalaController?.SetScale(1.2f);
                imageController?.SetRotationSpeed(15f);
                break;

            case MandalaPhase.Resolution:
                mandalaController?.SetColor(new Color(0.9f, 0.9f, 1.0f));
                mandalaController?.SetScale(0.8f);
                imageController?.SetRotationSpeed(5f);
                imageController?.FadeOut();
                break;
        }
    }

    IEnumerator AnimateEmergenceFadeIn(float duration)
    {
        float time = 0f;
        imageController?.FadeOut();

        while (time < duration)
        {
            float t = time / duration;
            float alpha = Mathf.Lerp(0f, 1f, t);
            if (imageController?.spriteRenderer != null)
            {
                Color c = imageController.spriteRenderer.color;
                c.a = alpha;
                imageController.spriteRenderer.color = c;
            }
            time += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator AnimateEmergenceScale(float duration)
    {
        float time = 0f;
        float startScale = 0.2f;
        float endScale = 0.8f;

        while (time < duration)
        {
            float t = time / duration;
            mandalaController?.SetScale(Mathf.Lerp(startScale, endScale, t));
            time += Time.deltaTime;
            yield return null;
        }

        mandalaController?.SetScale(endScale);
    }
}
