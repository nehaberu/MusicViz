using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Analyzes real-time audio input (microphone or music track) to estimate emotional "mood"
// using RMS and spectral centroid. Dynamically updates mandala visuals—sprite, tint, glow, scale, and background movement—based on the detected mood.
[RequireComponent(typeof(AudioSource))]
public class RealTimeMoodAnalyzer : MonoBehaviour
{
    public MandalaController mandalaController;
    public MandalaShaderController shaderController;
    public MandalaImageController imageController;
    public BackgroundPanner backgroundPanner;
    public bool useMicrophone = false;

    private string selectedMic;
    private AudioSource audioSource;

    // Arrays to hold real-time audio data
    private float[] spectrum = new float[128];
    private float[] samples = new float[128];

    // Enum defining emotional "mood" states
    private enum Mood
    {
        Emergence, Curiosity, Buildup, Peak,
        Descent, Resolution, Reflection, Meditation
    }

    private Mood currentMood;
    private Mood previousMood = Mood.Resolution;
    private float lastMoodChangeTime = 0f;
    private float minMoodDuration = 2.5f; // Minimum time between mood changes

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Use microphone input if enabled
        if (useMicrophone)
        {
            if (Microphone.devices.Length > 0)
            {
                selectedMic = Microphone.devices[0];
                audioSource.clip = Microphone.Start(selectedMic, true, 10, AudioSettings.outputSampleRate);
                audioSource.loop = true;
                while (!(Microphone.GetPosition(selectedMic) > 0)) { }
                audioSource.Play();
            }
            else
            {
                Debug.LogError("No microphone found!");
            }
        }
        else
        {
            audioSource.Play(); // Play audio clip normally if no microphone
        }

        // Warn if required references aren't set
        if (!mandalaController) Debug.LogWarning("⚠ MandalaController not assigned.");
        if (!shaderController) Debug.LogWarning("⚠ ShaderController not assigned.");
        if (!imageController) Debug.LogWarning("⚠ ImageController not assigned.");
    }

    void Update()
    {
        // Get real-time audio data
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Blackman);
        audioSource.GetOutputData(samples, 0);

        // Compute audio features
        float rms = Mathf.Sqrt(samples.Average(s => s * s)); // Root Mean Square (volume/intensity)
        float centroid = ComputeSpectralCentroid(spectrum);  // Brightness of sound
        float intensity = Mathf.Clamp01(rms * 25f);           // Scaled volume for visuals

        // Pass intensity to visual elements
        backgroundPanner?.SetIntensity(intensity);
        mandalaController?.SetRotationSpeed(intensity);

        // Detect and apply mood
        ApplyMoodBasedOnAudio(rms, centroid);
    }

    // Calculates the spectral centroid (perceived brightness) from FFT data
    float ComputeSpectralCentroid(float[] spectrum)
    {
        float num = 0f, denom = 0f;
        for (int i = 0; i < spectrum.Length; i++)
        {
            float freq = i * AudioSettings.outputSampleRate / 2 / spectrum.Length;
            num += freq * spectrum[i];
            denom += spectrum[i];
        }
        return denom > 0 ? num / denom / 20000f : 0f; // Normalize for easier thresholding
    }

    // Determines the current mood based on audio features and applies corresponding visuals
    void ApplyMoodBasedOnAudio(float rms, float centroid)
    {
        if (!mandalaController || !shaderController || !imageController) return;

        // Mood classification based on loudness and spectral brightness
        if (rms < 0.015f)
            currentMood = Mood.Meditation;
        else if (rms < 0.03f && centroid < 0.2f)
            currentMood = Mood.Emergence;
        else if (rms < 0.06f && centroid < 0.35f)
            currentMood = Mood.Curiosity;
        else if (rms < 0.1f && centroid < 0.5f)
            currentMood = Mood.Buildup;
        else if (rms < 0.15f)
            currentMood = Mood.Peak;
        else if (rms < 0.2f)
            currentMood = Mood.Descent;
        else if (rms < 0.25f)
            currentMood = Mood.Resolution;
        else
            currentMood = Mood.Reflection;

        // Avoid rapid switching between moods
        if (currentMood == previousMood || Time.time - lastMoodChangeTime < minMoodDuration)
            return;

        previousMood = currentMood;
        lastMoodChangeTime = Time.time;

        // Apply mood-specific visual changes
        switch (currentMood)
        {
            case Mood.Emergence:
                shaderController.SetBaseColor(new Color(0.2f, 1f, 0.5f));
                imageController.SetTintColor(new Color(0.2f, 1f, 0.5f));
                shaderController.SetGlowIntensity(0.25f);
                mandalaController.SetScale(1.0f);
                imageController.SetPhaseSmooth(imageController.emergence, 1.5f);
                break;

            case Mood.Curiosity:
                shaderController.SetBaseColor(new Color(0f, 0.8f, 0.9f));
                imageController.SetTintColor(new Color(0f, 0.8f, 0.9f));
                shaderController.SetGlowIntensity(0.4f);
                mandalaController.SetScale(1.2f);
                imageController.SetPhaseSmooth(imageController.curiosity, 1.5f);
                break;

            case Mood.Buildup:
                shaderController.SetBaseColor(Color.yellow);
                imageController.SetTintColor(Color.yellow);
                shaderController.SetGlowIntensity(0.55f);
                mandalaController.SetScale(1.4f);
                imageController.SetPhaseSmooth(imageController.buildup, 1.5f);
                break;

            case Mood.Peak:
                shaderController.SetBaseColor(new Color(1f, 0.3f, 0.1f));
                imageController.SetTintColor(new Color(1f, 0.3f, 0.1f));
                shaderController.SetGlowIntensity(0.8f);
                mandalaController.SetScale(1.8f);
                imageController.SetPhaseSmooth(imageController.peak, 1.5f);
                break;

            case Mood.Descent:
                shaderController.SetBaseColor(new Color(0.8f, 0.5f, 1f));
                imageController.SetTintColor(new Color(0.8f, 0.5f, 1f));
                shaderController.SetGlowIntensity(0.3f);
                mandalaController.SetScale(1.2f);
                imageController.SetPhaseSmooth(imageController.descent, 1.5f);
                break;

            case Mood.Resolution:
                shaderController.SetBaseColor(Color.white);
                imageController.SetTintColor(Color.white);
                shaderController.SetGlowIntensity(0.1f);
                mandalaController.SetScale(0.9f);
                imageController.SetPhaseSmooth(imageController.resolution, 1.5f);
                break;

            case Mood.Reflection:
                shaderController.SetBaseColor(new Color(1f, 0.5f, 0.9f));
                imageController.SetTintColor(new Color(1f, 0.5f, 0.9f));
                shaderController.SetGlowIntensity(0.2f);
                mandalaController.SetScale(1.1f);
                imageController.SetPhaseSmooth(imageController.reflection, 1.5f);
                break;

            case Mood.Meditation:
                shaderController.SetBaseColor(new Color(0.5f, 0.7f, 1f));
                imageController.SetTintColor(new Color(0.5f, 0.7f, 1f));
                shaderController.SetGlowIntensity(0.15f);
                mandalaController.SetScale(0.85f);
                imageController.SetPhaseSmooth(imageController.meditation, 1.5f);
                break;
        }

        // Debug log to monitor transitions
        Debug.Log($"🎵 Mood changed to: {currentMood} | RMS: {rms:F3}, Centroid: {centroid:F3}");
    }
}
