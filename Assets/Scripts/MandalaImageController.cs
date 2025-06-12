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

        // Handle dissolve (fade in/out)
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

        switch (phase.ToLower())
        {
            case "emergence": spriteRenderer.sprite = emergence; break;
            case "curiosity": spriteRenderer.sprite = curiosity; break;
            case "buildup": spriteRenderer.sprite = buildup; break;
            case "peak": spriteRenderer.sprite = peak; break;
            case "descent": spriteRenderer.sprite = descent; break;
            case "resolution": spriteRenderer.sprite = resolution; break;
        }

        // Automatically fade in every time the phase changes
        FadeIn();
    }

    public void FadeIn() => targetAlpha = 1f;
    public void FadeOut() => targetAlpha = 0f;
    public void SetRotationSpeed(float speed) => rotationSpeed = speed;
}
