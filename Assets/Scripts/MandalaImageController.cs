using System.Collections;
using UnityEngine;

//handles the dynamic behavior of the mandala image, including smooth phase transitions (crossfade), rotation, alpha fade in/out, 
//and optional pulse particle effects. 

public class MandalaImageController : MonoBehaviour
{
    [Header("Mandala Sprites")]
    public SpriteRenderer spriteRenderer;
    public Sprite emergence, curiosity, buildup, peak, descent, resolution;
    public Sprite reflection, meditation;

    [Header("Transition Settings")]
    [SerializeField] private float transitionDuration = 1.5f;  // Default crossfade time
    [SerializeField] private float rotationSpeed = 20f;        // Speed of mandala rotation
    [SerializeField] private float fadeSpeed = 1.5f;           // Speed of alpha fade

    [Header("Optional Effects")]
    [SerializeField] private ParticleSystem pulseEffect;       // Optional visual burst on transition

    private float targetAlpha = 1f;
    private Coroutine currentTransition;
    private Color tintColor = Color.white;

    private void Start()
    {
        if (spriteRenderer == null)
            Debug.LogWarning("SpriteRenderer not assigned!");
    }

    private void Update()
    {
        // Continuous rotation
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // Smoothly interpolate alpha toward targetAlpha
        if (spriteRenderer != null)
        {
            Color current = spriteRenderer.color;
            float newAlpha = Mathf.Lerp(current.a, targetAlpha, Time.deltaTime * fadeSpeed);
            spriteRenderer.color = new Color(tintColor.r, tintColor.g, tintColor.b, newAlpha);
        }
    }

   
    //Sets the next mandala sprite with a smooth crossfade transition.

    public void SetPhaseSmooth(Sprite newSprite, float duration)
    {
        if (spriteRenderer == null || newSprite == null) return;
        if (spriteRenderer.sprite == newSprite) return;

        if (currentTransition != null)
            StopCoroutine(currentTransition);

        currentTransition = StartCoroutine(CrossFadeTransition(newSprite, duration));
    }

    //Set the base color tint for the sprite (excluding alpha).
    public void SetTintColor(Color color) => tintColor = color;

    //Set alpha immediately without transition.
    public void SetAlpha(float a)
    {
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = a;
            spriteRenderer.color = c;
        }
    }

    //Adjust rotation speed of the mandala
    public void SetRotationSpeed(float speed) => rotationSpeed = speed;

    //Begin a gradual fade in
    public void FadeIn() => targetAlpha = 1f;

    //Begin a gradual fade out
    public void FadeOut() => targetAlpha = 0f;


    //Coroutine to handle smooth crossfade between current and new sprite.
    //An overlay temporarily displays the old sprite for blending.
  
    private IEnumerator CrossFadeTransition(Sprite newSprite, float duration)
    {
        // Create an overlay object to display the old sprite during fade
        GameObject overlayObj = new GameObject("MandalaTransitionOverlay");
        overlayObj.transform.SetParent(transform.parent, false);
        SpriteRenderer overlayRenderer = overlayObj.AddComponent<SpriteRenderer>();

        overlayRenderer.sprite = spriteRenderer.sprite;
        overlayRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
        overlayRenderer.color = spriteRenderer.color;
        overlayRenderer.transform.localScale = transform.localScale;

        // Assign new sprite with 0 alpha
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
        TriggerPulse();  // Optional visual effect
        currentTransition = null;
    }

   
    //Triggers a particle pulse effect if one is assigned.

    private void TriggerPulse()
    {
        if (pulseEffect != null)
        {
            pulseEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            pulseEffect.Play();
        }
    }
}
