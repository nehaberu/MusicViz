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
        if (spriteRenderer == null)
        {
            Debug.LogWarning("🚫 SpriteRenderer is null in SetPhase!");
            return;
        }

        Sprite selectedSprite = null;

        switch (phase.ToLower())
        {
            case "emergence": selectedSprite = emergence; break;
            case "curiosity": selectedSprite = curiosity; break;
            case "buildup": selectedSprite = buildup; break;
            case "peak": selectedSprite = peak; break;
            case "descent": selectedSprite = descent; break;
            case "resolution": selectedSprite = resolution; break;
            default:
                Debug.LogWarning($"❓ Unknown phase: {phase}");
                return;
        }

        spriteRenderer.sprite = selectedSprite;

        Debug.Log($"🖼 Sprite for phase '{phase}' set to: {selectedSprite?.name ?? "null"}");
        FadeIn();  // Optional
    }



    public void FadeIn() => targetAlpha = 1f;
    public void FadeOut() => targetAlpha = 0f;
    public void SetRotationSpeed(float speed) => rotationSpeed = speed;
}
