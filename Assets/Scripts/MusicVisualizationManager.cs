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
    
    [Header("Visualization Section Markers")]
    // Based on your specification document timestamps
    [SerializeField] private float emergenceStartTime = 0f;
    [SerializeField] private float curiosityStartTime = 45f;
    [SerializeField] private float buildupStartTime = 80f;  // 1:20
    [SerializeField] private float peakStartTime = 130f;    // 2:10
    [SerializeField] private float descentStartTime = 180f; // 3:00
    [SerializeField] private float resolutionStartTime = 240f; // 4:00
    
    // State tracking
    private enum MandalaPhase { Emergence, Curiosity, Buildup, Peak, Descent, Resolution }
    private MandalaPhase currentPhase = MandalaPhase.Emergence;
    private bool isPlaying = false;
    
    // Start is called before the first frame update
    [Header("Mandala Shape Settings")]
    [SerializeField] private float radius = 5f;
    [SerializeField] private float innerRadius = 1f;
    [SerializeField] private float intensity = 0.3f;

    
    void Start()
    {
        // Initialize audio source if needed
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
            
        if (audioSource != null && musicTrack != null)
            audioSource.clip = musicTrack;
            
        // Connect the timeline if available
        if (timelineDirector != null && timelineAsset != null)
            timelineDirector.playableAsset = timelineAsset;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) Play();
        if (!isPlaying || audioSource == null || !audioSource.isPlaying) return;
        
        // Get current playback time
        float currentTime = audioSource.time;
        
        // Update the current phase based on time
        UpdateCurrentPhase(currentTime);
        
        // Apply visual effects based on current phase
        ApplyVisualEffects();
    }
    
    private void UpdateCurrentPhase(float currentTime)
    {
        // Determine which phase we're in based on the time
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
        if (mandalaController == null) return;
        
        // Apply different effects based on current phase
        switch (currentPhase)
        {
            case MandalaPhase.Emergence:
                // Soft blue, small symmetrical mandala
                mandalaController.SetColor(new Color(0.2f, 0.4f, 0.8f)); // Soft blue
                mandalaController.SetScale(0.7f);
                UpdateMandalaComplexity(8, 3); // Simple form
                break;
                
            case MandalaPhase.Curiosity:
                // Shift from blue to teal, slight expansion
                mandalaController.SetColor(new Color(0.2f, 0.7f, 0.8f)); // Teal
                mandalaController.SetScale(1.0f);
                UpdateMandalaComplexity(10, 4);
                break;
                
            case MandalaPhase.Buildup:
                // More intricate edges, golden glow emerges
                mandalaController.SetColor(new Color(0.4f, 0.6f, 0.2f, 0.9f)); // Green-gold
                mandalaController.SetScale(1.2f);
                UpdateMandalaComplexity(12, 5);
                break;
                
            case MandalaPhase.Peak:
                // Explosion into reds/golds, complex fractals
                mandalaController.SetColor(new Color(0.9f, 0.4f, 0.1f)); // Red-gold
                mandalaController.SetScale(1.5f);
                UpdateMandalaComplexity(16, 8); // Maximum complexity
                break;
                
            case MandalaPhase.Descent:
                // Colors cool to lilac, motion slows
                mandalaController.SetColor(new Color(0.6f, 0.4f, 0.8f)); // Lilac
                mandalaController.SetScale(1.2f);
                UpdateMandalaComplexity(10, 5);
                break;
                
            case MandalaPhase.Resolution:
                // Minimal white spiral
                mandalaController.SetColor(new Color(0.9f, 0.9f, 1.0f)); // Nearly white
                mandalaController.SetScale(0.8f);
                UpdateMandalaComplexity(6, 2); // Simple form again
                break;
        }
    }
    
    private void UpdateMandalaComplexity(int segments, int layers)
    {
        // Update complexity in the controller (for animation parameters)
        if (mandalaController != null)
            mandalaController.SetComplexity(segments, layers);
            
        // Update the actual mandala mesh generator if available
        if (mandalaGenerator != null)
        {
            float variation = currentPhase == MandalaPhase.Peak ? 0.5f : 
                             (currentPhase == MandalaPhase.Buildup ? 0.3f : 0.1f);
                             
            mandalaGenerator.UpdateMandala(radius, innerRadius, segments, intensity);
        }
    }
    
    // Control methods
    public void Play()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
            
            // Start timeline if available
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
            
            // Pause timeline if available
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
            
            // Stop timeline if available
            if (timelineDirector != null)
                timelineDirector.Stop();
                
            isPlaying = false;
            
            // Reset to beginning phase
            currentPhase = MandalaPhase.Emergence;
            ApplyVisualEffects();
        }
    }
}