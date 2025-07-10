using UnityEngine;

public class BackgroundPanner : MonoBehaviour
{
    public float scrollSpeedX = 0.01f;
    public float scrollSpeedY = 0.01f;

    private Material mat;
    private Vector2 offset = Vector2.zero;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
            mat = renderer.material;
    }

    void Update()
    {
        if (mat != null)
        {
            offset.x += scrollSpeedX * Time.deltaTime;
            offset.y += scrollSpeedY * Time.deltaTime;
            mat.mainTextureOffset = offset;
        }
    }
}
