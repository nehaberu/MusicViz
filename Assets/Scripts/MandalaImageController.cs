using System.Collections;
using UnityEngine;

public class MandalaImageController : MonoBehaviour
{
    [Header("Mandala Sprites")]
    public SpriteRenderer spriteRenderer;
    public Sprite emergence, curiosity, buildup, peak, descent, resolution;
    public Sprite reflection, meditation;

    [Header("Transition Settings")]
    [SerializeField] private float transitionDuration = 1.5f;
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private float fadeSpeed = 1.5f;

    [Header("Optional Effects")]
    [SerializeField] private ParticleSystem pulseEffect;

    private float targetAlpha = 1f;
    private Coroutine currentTransition;
    private Color tintColor = Color.white;

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
            float newAlpha = Mathf.Lerp(current.a, targetAlpha, Time.deltaTime * fadeSpeed);
            spriteRenderer.color = new Color(tintColor.r, tintColor.g, tintColor.b, newAlpha);
        }
    }

    public void SetPhaseSmooth(Sprite newSprite, float duration)
    {
        if (spriteRenderer == null || newSprite == null) return;
        if (spriteRenderer.sprite == newSprite) return;

        if (currentTransition != null)
            StopCoroutine(currentTransition);

        currentTransition = StartCoroutine(CrossFadeTransition(newSprite, duration));
    }

    public void SetTintColor(Color color) => tintColor = color;
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

    private IEnumerator CrossFadeTransition(Sprite newSprite, float duration)
    {
        GameObject overlayObj = new GameObject("MandalaTransitionOverlay");
        overlayObj.transform.SetParent(transform.parent, false);
        SpriteRenderer overlayRenderer = overlayObj.AddComponent<SpriteRenderer>();

        overlayRenderer.sprite = spriteRenderer.sprite;
        overlayRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
        overlayRenderer.color = spriteRenderer.color;
        overlayRenderer.transform.localScale = transform.localScale;

        spriteRenderer.sprite = newSprite;
        spriteRenderer.color = new Color(tintColor.r, tintColor.g, tintColor.b, 0f);

        float time = 0f;
        while (time < duration)
        {
            float t = time / duration;
            float fadeInAlpha = Mathf.SmoothStep(0f, 1f, t);
            float fadeOutAlpha = 1f - fadeInAlpha;

            spriteRenderer.color = new Color(tintColor.r, tintColor.g, tintColor.b, fadeInAlpha);
            overlayRenderer.color = new Color(overlayRenderer.color.r, overlayRenderer.color.g, overlayRenderer.color.b, fadeOutAlpha);

            time += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = new Color(tintColor.r, tintColor.g, tintColor.b, 1f);
        Destroy(overlayObj);
        TriggerPulse();
        currentTransition = null;
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
