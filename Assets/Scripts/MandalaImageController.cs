using System.Collections;
using UnityEngine;

public class MandalaImageController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    // 🌸 Add all 8 sprites here
    public Sprite emergence, curiosity, buildup, peak;
    public Sprite descent, resolution, reflection, meditation;

    private float rotationSpeed = 20f;
    private float targetAlpha = 1f;
    private float fadeSpeed = 1.5f;

    [SerializeField] private float transitionDuration = 1.5f;

    private void Update()
    {
        // 🌀 Auto-rotate
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // ✨ Smooth alpha fade
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

        Material currentMat = spriteRenderer.sharedMaterial;
        spriteRenderer.sprite = selectedSprite;
        spriteRenderer.sharedMaterial = currentMat;

        Debug.Log($"🖼 Sprite set for phase '{phase}' → {selectedSprite?.name}");
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
        float time = 0f;
        Color c = spriteRenderer.color;
        float startAlpha = c.a;

        Vector3 originalScale = transform.localScale;
        Vector3 popScale = originalScale * 1.1f;
        Vector3 miniScale = originalScale * 0.4f;

        // 🌘 Fade out and shrink
        while (time < duration / 2f)
        {
            float t = Mathf.SmoothStep(0f, 1f, time / (duration / 2f));
            c.a = Mathf.Lerp(startAlpha, 0f, t);
            spriteRenderer.color = c;
            transform.localScale = Vector3.Lerp(originalScale, miniScale, t);
            time += Time.deltaTime;
            yield return null;
        }

        c.a = 0f;
        spriteRenderer.color = c;

        Material currentMat = spriteRenderer.sharedMaterial;
        spriteRenderer.sprite = newSprite;
        spriteRenderer.sharedMaterial = currentMat;

        transform.localScale = popScale;

        // 🌕 Fade in and pop back
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

        c.a = 1f;
        spriteRenderer.color = c;
        transform.localScale = originalScale;
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
            case "reflection": return reflection;
            case "meditation": return meditation;
            default: return null;
        }
    }

    public void FadeIn() => targetAlpha = 1f;
    public void FadeOut() => targetAlpha = 0f;
    public void SetRotationSpeed(float speed) => rotationSpeed = speed;
}
