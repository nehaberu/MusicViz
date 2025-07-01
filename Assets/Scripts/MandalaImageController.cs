using System.Collections;
using UnityEngine;

public class MandalaImageController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public Sprite emergence, curiosity, buildup, peak, descent, resolution;

    private float rotationSpeed = 20f;
    private float targetAlpha = 1f;
    private float fadeSpeed = 1.5f;

    private void Update()
    {
        // Auto-rotate
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // Handle dissolve fade in/out
        if (spriteRenderer != null)
        {
            Color current = spriteRenderer.color;
            current.a = Mathf.Lerp(current.a, targetAlpha, Time.deltaTime * fadeSpeed);
            spriteRenderer.color = current;
        }
    }

    public void SetPhase(string phase)
    {
        if (spriteRenderer == null)
        {
            Debug.LogWarning("🚫 SpriteRenderer is null in SetPhase!");
            return;
        }

        Sprite selectedSprite = GetPhaseSprite(phase);
        if (selectedSprite == null)
        {
            Debug.LogWarning($"❓ Unknown or null sprite for phase: {phase}");
            return;
        }

        spriteRenderer.sprite = selectedSprite;
        Debug.Log($"🖼 Sprite for phase '{phase}' set to: {selectedSprite?.name ?? "null"}");

        FadeIn();
    }

    public void SetPhaseSmooth(Sprite newSprite, float duration)
    {
        StartCoroutine(SmoothTransition(newSprite, duration));
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


    IEnumerator SmoothTransition(Sprite newSprite, float duration)
    {
        // Fade out current sprite
        float time = 0f;
        Color c = spriteRenderer.color;
        float startAlpha = c.a;

        while (time < duration / 2f)
        {
            float t = time / (duration / 2f);
            c.a = Mathf.Lerp(startAlpha, 0f, t);
            spriteRenderer.color = c;
            time += Time.deltaTime;
            yield return null;
        }

        c.a = 0f;
        spriteRenderer.color = c;
        spriteRenderer.sprite = newSprite;

        // Scale down temporarily
        transform.localScale = Vector3.one * 0.4f;

        // Fade in and scale up
        time = 0f;
        while (time < duration / 2f)
        {
            float t = time / (duration / 2f);
            c.a = Mathf.Lerp(0f, 1f, t);
            spriteRenderer.color = c;

            float scale = Mathf.Lerp(0.4f, 1f, t);
            transform.localScale = Vector3.one * scale;

            time += Time.deltaTime;
            yield return null;
        }

        c.a = 1f;
        spriteRenderer.color = c;
        transform.localScale = Vector3.one;
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
            default: return null;
        }
    }

    public void FadeIn() => targetAlpha = 1f;
    public void FadeOut() => targetAlpha = 0f;
    public void SetRotationSpeed(float speed) => rotationSpeed = speed;
}
