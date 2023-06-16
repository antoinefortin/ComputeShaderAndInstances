using UnityEngine;

public class DisplaceVertices : MonoBehaviour
{
    public ComputeShader displacementShader;
    public Texture2D displacementMap;
    public float displacementScale = 1.0f;
    public float randomOffset = 0.01f;

    private ComputeBuffer vertexBuffer;
    private Vector3[] vertices;

    private MeshFilter meshFilter;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();

        // Create a clone of the mesh to avoid modifying the shared mesh asset
        meshFilter.mesh = Instantiate(meshFilter.sharedMesh);

        // Copy the mesh vertices to the array
        vertices = meshFilter.mesh.vertices;

        Debug.Log(vertices.Length);

        // Create the compute buffer and set the data
        vertexBuffer = new ComputeBuffer(vertices.Length, sizeof(float) * 3);
        vertexBuffer.SetData(vertices);

        // Bind the compute buffer to the shader
        displacementShader.SetBuffer(0, "Vertices", vertexBuffer);

        // Bind the displacement map and scale to the shader
        displacementShader.SetTexture(0, "DisplacementMap", displacementMap);
        displacementShader.SetFloat("DisplacementScale", displacementScale);

    }

    void Update()
    {
        // Dispatch the compute shader
        displacementShader.Dispatch(0, vertices.Length / 8, 1, 1);

        // Get the data back from the GPU and apply to the mesh
        vertexBuffer.GetData(vertices);
        meshFilter.mesh.vertices = vertices;

        // Recalculate bounds and normals
        meshFilter.mesh.RecalculateBounds();
        meshFilter.mesh.RecalculateNormals();
    }

    void OnDestroy()
    {
        // Release the compute buffer
        vertexBuffer.Release();
    }
}
