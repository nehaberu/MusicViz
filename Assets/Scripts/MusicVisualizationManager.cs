using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private float curiosityStartTime = 2f;
    [SerializeField] private float buildupStartTime = 4f;
    [SerializeField] private float peakStartTime = 6f;
    [SerializeField] private float descentStartTime = 180f;
    [SerializeField] private float resolutionStartTime = 240f;

    private enum MandalaPhase { Emergence, Curiosity, Buildup, Peak, Descent, Resolution }
    private MandalaPhase currentPhase = MandalaPhase.Emergence;
    private MandalaPhase previousPhase = MandalaPhase.Emergence;
    private bool isPlaying = false;

    void Start()
    {
        Debug.Log("✅ MusicVisualizationManager is running");

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource != null && musicTrack != null)
            audioSource.clip = musicTrack;

        if (timelineDirector != null && timelineAsset != null)
            timelineDirector.playableAsset = timelineAsset;

        Play(); // Auto-start for debugging; remove if not needed
    }

    void Update()
    {
        Debug.Log("📍 Update is running");

        //if (!isPlaying || audioSource == null || !audioSource.isPlaying)
            //return;

        float currentTime = audioSource.time;
        Debug.Log("🎵 Music Time: " + currentTime.ToString("F2"));

        UpdateCurrentPhase(currentTime);

        if (currentPhase != previousPhase)
        {
            Debug.Log("🔁 Phase changed: " + previousPhase + " → " + currentPhase);
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
        Debug.Log("✨ Applying visuals for phase: " + currentPhase.ToString());

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
                mandalaController?.SetScale(0.7f);
                imageController?.SetRotationSpeed(10f);
                imageController?.FadeIn();
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

    public void Play()
    {
    if (audioSource != null && !audioSource.isPlaying)
    {
        Debug.Log("▶️ Play() called");
        audioSource.Play();
        timelineDirector?.Play();
        isPlaying = true;
    }
    else
    {
        Debug.Log("⚠️ Play skipped: already playing or audioSource is null");
    }
    }


    public void Pause()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            Debug.Log("⏸ Pausing music...");
            audioSource.Pause();
            timelineDirector?.Pause();
            isPlaying = false;
        }
    }

    public void Stop()
    {
        if (audioSource != null)
        {
            Debug.Log("⏹ Stopping music...");
            audioSource.Stop();
            timelineDirector?.Stop();
            isPlaying = false;
            currentPhase = MandalaPhase.Emergence;
            previousPhase = MandalaPhase.Emergence;
            ApplyVisualEffects();
        }
    }

    private void TogglePlayPause()
    {
        if (isPlaying) Pause();
        else Play();
    }

    public void PlayButton()
    {
        Debug.Log("🟢 Play button clicked");
        Play();
    }
}
