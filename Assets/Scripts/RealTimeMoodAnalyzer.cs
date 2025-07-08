using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class RealTimeMoodAnalyzer : MonoBehaviour
{
    public MandalaController mandalaController;
    public MandalaShaderController shaderController;

    private AudioSource audioSource;
    private float[] spectrum = new float[128];
    private float[] samples = new float[128];

    private enum Mood { Calm, Curious, Intense, Chaotic, Peaceful }
    private Mood currentMood;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (mandalaController == null)
            Debug.LogWarning("⚠️ MandalaController not assigned.");
        if (shaderController == null)
            Debug.LogWarning("⚠️ MandalaShaderController not assigned.");
    }

    void Update()
    {
        // 🎧 Get current audio data
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Blackman);
        audioSource.GetOutputData(samples, 0);

        // 📈 Calculate loudness (RMS)
        float rms = Mathf.Sqrt(samples.Average(s => s * s));

        // 🔊 Calculate spectral centroid (for mood logic)
        float centroid = ComputeSpectralCentroid(spectrum);

        // 🌀 STEP 1: Map intensity to 0–1
        float intensity = Mathf.Clamp01(rms * 25f);  // Tweak 25f to control sensitivity

        // 🌀 STEP 2: Control mandala rotation speed
        mandalaController?.SetRotationSpeed(intensity);

        // 🎨 STEP 3: Apply mood-based visuals
        ApplyMoodBasedOnRMSAndCentroid(rms, centroid);
    }

    float ComputeSpectralCentroid(float[] spectrum)
    {
        float num = 0f, denom = 0f;
        for (int i = 0; i < spectrum.Length; i++)
        {
            float freq = i * AudioSettings.outputSampleRate / 2 / spectrum.Length;
            num += freq * spectrum[i];
            denom += spectrum[i];
        }
        return denom > 0 ? num / denom / 20000f : 0f; // Normalize
    }

    void ApplyMoodBasedOnRMSAndCentroid(float rms, float centroid)
    {
        if (mandalaController == null || shaderController == null) return;

        // Classify mood
        if (rms < 0.03f && centroid < 0.2f)
            currentMood = Mood.Calm;
        else if (rms < 0.06f && centroid < 0.35f)
            currentMood = Mood.Curious;
        else if (rms < 0.1f)
            currentMood = Mood.Intense;
        else
            currentMood = Mood.Chaotic;

        // Apply mood
        switch (currentMood)
        {
            case Mood.Calm:
                shaderController.SetBaseColor(Color.blue);
                shaderController.SetGlowIntensity(0.2f);
                mandalaController.SetScale(1.0f);
                break;

            case Mood.Curious:
                shaderController.SetBaseColor(new Color(0f, 0.8f, 0.8f)); // teal
                shaderController.SetGlowIntensity(0.4f);
                mandalaController.SetScale(1.3f);
                break;

            case Mood.Intense:
                shaderController.SetBaseColor(Color.yellow);
                shaderController.SetGlowIntensity(0.6f);
                mandalaController.SetScale(1.7f);
                break;

            case Mood.Chaotic:
                shaderController.SetBaseColor(Color.red);
                shaderController.SetGlowIntensity(0.9f);
                mandalaController.SetScale(2.0f);
                break;

            case Mood.Peaceful:
                shaderController.SetBaseColor(Color.white);
                shaderController.SetGlowIntensity(0.1f);
                mandalaController.SetScale(0.8f);
                break;
        }

        Debug.Log($"🎵 Mood: {currentMood}, RMS: {rms:F3}, Centroid: {centroid:F3}");
    }
}
