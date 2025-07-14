using UnityEngine;

// Moves the background texture slowly based on audio intensity
[RequireComponent(typeof(Renderer))]
public class BackgroundPanner : MonoBehaviour
{
    private Material mat;
    private Vector2 offset = Vector2.zero;

    [Range(0f, 1f)]
    public float audioIntensity = 0f;

    public float baseSpeed = 0.01f;  
    public float maxSpeed = 0.08f;   

    void Start()
    {
        // Get the material of the object
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        // Adjust speed based on intensity
        float speed = Mathf.Lerp(baseSpeed, maxSpeed, audioIntensity);

        // Move the texture to create a slow pan effect
        offset += new Vector2(speed * Time.deltaTime, speed * 0.5f * Time.deltaTime);
        mat.mainTextureOffset = offset;
    }

    public void SetIntensity(float intensity)
    {
        // Clamp between 0 and 1
        audioIntensity = Mathf.Clamp01(intensity);
    }
}
