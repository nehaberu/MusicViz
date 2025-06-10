using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MandalaGenerator : MonoBehaviour
{
    [Range(3, 360)]
    public int segments = 60;
    public float radius = 5f;
    public float innerRadius = 1f;

    private Mesh mesh;

    private void Start()
    {
        Debug.Log("MandalaGenerator Start() called!");
        GenerateMandala();
    }


    // Simple update with no parameters
    public void UpdateMandala()
    {
        GenerateMandala();
    }

    // Main update with 4 parameters
    public void UpdateMandala(float newRadius, float newInnerRadius, int newSegments, float intensity)
    {
        radius = newRadius * (1f + intensity); // optional intensity-based scaling
        innerRadius = newInnerRadius;
        segments = Mathf.Clamp(newSegments, 3, 360);
        GenerateMandala();
    }

    private void GenerateMandala()
    {
        mesh = new Mesh();
        Vector3[] vertices = new Vector3[segments * 2];
        int[] triangles = new int[segments * 6];

        float angleStep = 360f / segments;

        for (int i = 0; i < segments; i++)
        {
            float angleRad = Mathf.Deg2Rad * i * angleStep;

            Vector3 outer = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0) * radius;
            Vector3 inner = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0) * innerRadius;

            vertices[i * 2] = inner;
            vertices[i * 2 + 1] = outer;
        }

        for (int i = 0; i < segments; i++)
        {
            int i0 = i * 2;
            int i1 = i * 2 + 1;
            int i2 = (i * 2 + 2) % vertices.Length;
            int i3 = (i * 2 + 3) % vertices.Length;

            triangles[i * 6] = i0;
            triangles[i * 6 + 1] = i2;
            triangles[i * 6 + 2] = i1;

            triangles[i * 6 + 3] = i1;
            triangles[i * 6 + 4] = i2;
            triangles[i * 6 + 5] = i3;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
    }
}
