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
    [SerializeField] private MandalaGenerator mandalaGenerator;
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
    private bool isPlaying = false;

    [Header("Mandala Shape Settings")]
    [SerializeField] private float radius = 5f;
    [SerializeField] private float innerRadius = 1f;
    [SerializeField] private float intensity = 0.3f;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource != null && musicTrack != null)
            audioSource.clip = musicTrack;

        if (timelineDirector != null && timelineAsset != null)
            timelineDirector.playableAsset = timelineAsset;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) Play();
        if (!isPlaying || audioSource == null || !audioSource.isPlaying) return;

        float currentTime = audioSource.time;
        UpdateCurrentPhase(currentTime);
        ApplyVisualEffects();
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
        if (imageController != null)
        {
            imageController.SetPhase(currentPhase.ToString());
        }

        switch (currentPhase)
        {
            case MandalaPhase.Emergence:
                mandalaController.SetColor(new Color(0.2f, 0.4f, 0.8f));
                mandalaController.SetScale(0.7f);
                UpdateMandalaComplexity(8, 3);
                if (imageController != null)
                {
                    imageController.SetPhase("emergence");
                    imageController.SetRotationSpeed(10f);
                    imageController.FadeIn();
                }
                break;

            case MandalaPhase.Curiosity:
                mandalaController.SetColor(new Color(0.2f, 0.7f, 0.8f));
                mandalaController.SetScale(1.0f);
                UpdateMandalaComplexity(10, 4);
                if (imageController != null)
                {
                    imageController.SetPhase("curiosity");
                    imageController.SetRotationSpeed(20f);
                }
                break;

            case MandalaPhase.Buildup:
                mandalaController.SetColor(new Color(0.4f, 0.6f, 0.2f, 0.9f));
                mandalaController.SetScale(1.2f);
                UpdateMandalaComplexity(12, 5);
                if (imageController != null)
                {
                    imageController.SetPhase("buildup");
                    imageController.SetRotationSpeed(30f);
                }
                break;

            case MandalaPhase.Peak:
                mandalaController.SetColor(new Color(0.9f, 0.4f, 0.1f));
                mandalaController.SetScale(1.5f);
                UpdateMandalaComplexity(16, 8);
                if (imageController != null)
                {
                    imageController.SetPhase("peak");
                    imageController.SetRotationSpeed(40f);
                }
                break;

            case MandalaPhase.Descent:
                mandalaController.SetColor(new Color(0.6f, 0.4f, 0.8f));
                mandalaController.SetScale(1.2f);
                UpdateMandalaComplexity(10, 5);
                if (imageController != null)
                {
                    imageController.SetPhase("descent");
                    imageController.SetRotationSpeed(15f);
                }
                break;

            case MandalaPhase.Resolution:
                mandalaController.SetColor(new Color(0.9f, 0.9f, 1.0f));
                mandalaController.SetScale(0.8f);
                UpdateMandalaComplexity(6, 2);
                if (imageController != null)
                {
                    imageController.SetPhase("resolution");
                    imageController.SetRotationSpeed(5f);
                    imageController.FadeOut();
                }
                break;
        }
    }

    private void UpdateMandalaComplexity(int segments, int layers)
    {
        if (mandalaController != null)
            mandalaController.SetComplexity(segments, layers);

        if (mandalaGenerator != null)
        {
            float variation = currentPhase == MandalaPhase.Peak ? 0.5f :
                             (currentPhase == MandalaPhase.Buildup ? 0.3f : 0.1f);

            mandalaGenerator.UpdateMandala(radius, innerRadius, segments, intensity);
        }
    }

    public void Play()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();

            if (timelineDirector != null)
                timelineDirector.Play();

            isPlaying = true;
        }
    }

    public void Pause()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Pause();

            if (timelineDirector != null)
                timelineDirector.Pause();

            isPlaying = false;
        }
    }

    public void Stop()
    {
        if (audioSource != null)
        {
            audioSource.Stop();

            if (timelineDirector != null)
                timelineDirector.Stop();

            isPlaying = false;
            currentPhase = MandalaPhase.Emergence;
            ApplyVisualEffects();
        }
    }
}
