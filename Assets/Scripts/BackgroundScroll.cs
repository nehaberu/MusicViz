using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class BackgroundPanner : MonoBehaviour
{
    private Material mat;
    private Vector2 offset = Vector2.zero;

    [Range(0f, 1f)]
    public float audioIntensity = 0f;

    public float baseSpeed = 0.01f;      // Movement at rest
    public float maxSpeed = 0.08f;       // Max speed when music is intense

    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        float speed = Mathf.Lerp(baseSpeed, maxSpeed, audioIntensity);
        offset += new Vector2(speed * Time.deltaTime, speed * 0.5f * Time.deltaTime);
        mat.mainTextureOffset = offset;
    }

    public void SetIntensity(float intensity)
    {
        audioIntensity = Mathf.Clamp01(intensity);
    }
}
