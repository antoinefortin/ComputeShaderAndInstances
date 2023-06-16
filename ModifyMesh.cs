using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyMesh : MonoBehaviour
{
    public ComputeShader computeShader;
    ComputeBuffer vertexBuffer;
    Vector3[] vertexArray;

    Mesh mesh;
    MeshFilter meshFilter;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;

        // Copy the mesh vertices into the vertexArray
        vertexArray = mesh.vertices;

        // Create the compute buffer and set the data
        vertexBuffer = new ComputeBuffer(vertexArray.Length, sizeof(float) * 3);
        vertexBuffer.SetData(vertexArray);

        // Bind the compute buffer to the shader
        computeShader.SetBuffer(0, "Vertices", vertexBuffer);
    }

    void Update()
    {
        // Dispatch the compute shader
        computeShader.Dispatch(0, vertexArray.Length / 8, 1, 1);

        // Get the data back from the GPU and apply to the mesh
        vertexBuffer.GetData(vertexArray);
        mesh.vertices = vertexArray;

        // Recalculate bounds and normals
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

    void OnDestroy()
    {
        // Release the compute buffer
        vertexBuffer.Release();
    }
}
