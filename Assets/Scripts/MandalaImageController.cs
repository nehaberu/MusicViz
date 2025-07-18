using System.Collections;
using UnityEngine;

public class MandalaImageController : MonoBehaviour
{
    [Header("Mandala Sprites")]
    public SpriteRenderer spriteRenderer;
    public Sprite emergence, curiosity, buildup, peak, descent, resolution;
    public Sprite reflection;   // ✅ Added
    public Sprite meditation;   // ✅ Added

    [Header("Transition Settings")]
    [SerializeField] private float transitionDuration = 1.5f;
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private float fadeSpeed = 1.5f;

    [Header("Optional Effects")]
    [SerializeField] private ParticleSystem pulseEffect;

    private float targetAlpha = 1f;
    private Coroutine currentTransition;

    private void Start()
    {
        if (spriteRenderer == null)
            Debug.LogWarning("🚫 SpriteRenderer not assigned!");
    }

    private void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        if (spriteRenderer != null)
        {
            Color current = spriteRenderer.color;
            current.a = Mathf.Lerp(current.a, targetAlpha, Time.deltaTime * fadeSpeed);
            spriteRenderer.color = current;
        }
    }

    public void SetPhase(string phase)
    {
        if (spriteRenderer == null) return;

        Sprite selectedSprite = GetPhaseSprite(phase);
        if (selectedSprite == null || spriteRenderer.sprite == selectedSprite)
            return;

        spriteRenderer.sprite = selectedSprite;
        FadeIn();

        Debug.Log($"🖼 Sprite set for phase '{phase}' → {selectedSprite?.name}");
    }

    public void SetPhaseSmooth(Sprite newSprite, float duration)
    {
        if (spriteRenderer == null || newSprite == null) return;
        if (spriteRenderer.sprite == newSprite)
            return;

        if (currentTransition != null)
            StopCoroutine(currentTransition);

        currentTransition = StartCoroutine(SmoothTransition(newSprite, duration));
    }

    public void SetAlpha(float a)
    {
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = a;
            spriteRenderer.color = c;
        }
    }

    public void SetRotationSpeed(float speed) => rotationSpeed = speed;
    public void FadeIn() => targetAlpha = 1f;
    public void FadeOut() => targetAlpha = 0f;

    private IEnumerator SmoothTransition(Sprite newSprite, float duration)
    {
        float time = 0f;
        Color c = spriteRenderer.color;
        float startAlpha = c.a;

        Vector3 originalScale = transform.localScale;
        Vector3 popScale = originalScale * 1.1f;
        Vector3 miniScale = originalScale * 0.4f;

        while (time < duration / 2f)
        {
            float t = Mathf.SmoothStep(0f, 1f, time / (duration / 2f));
            c.a = Mathf.Lerp(startAlpha, 0f, t);
            spriteRenderer.color = c;
            transform.localScale = Vector3.Lerp(originalScale, miniScale, t);
            time += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = new Color(c.r, c.g, c.b, 0f);
        spriteRenderer.sprite = newSprite;
        transform.localScale = popScale;

        TriggerPulse();

        time = 0f;
        while (time < duration / 2f)
        {
            float t = Mathf.SmoothStep(0f, 1f, time / (duration / 2f));
            c.a = Mathf.Lerp(0f, 1f, t);
            spriteRenderer.color = c;
            transform.localScale = Vector3.Lerp(popScale, originalScale, t);
            time += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = new Color(c.r, c.g, c.b, 1f);
        transform.localScale = originalScale;
        currentTransition = null;
    }

    private Sprite GetPhaseSprite(string phase)
    {
        switch (phase.ToLower())
        {
            case "emergence": return emergence;
            case "curiosity": return curiosity;
            case "buildup": return buildup;
            case "peak": return peak;
            case "descent": return descent;
            case "resolution": return resolution;
            case "reflection": return reflection;   // ✅ Added
            case "meditation": return meditation;   // ✅ Added
            default: return null;
        }
    }

    private void TriggerPulse()
    {
        if (pulseEffect != null)
        {
            pulseEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            pulseEffect.Play();
        }
    }
}
