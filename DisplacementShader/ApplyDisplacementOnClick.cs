using UnityEngine;

public class ApplyDisplacementOnClick : MonoBehaviour
{
    public ComputeShader displacementShader;
    public Texture2D displacementMap;
    public float displacementScale = 1.0f;
    public float randomOffset = 0.01f;

    void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hit an object
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the hit object has a MeshFilter and MeshRenderer
                MeshFilter meshFilter = hit.transform.GetComponent<MeshFilter>();
                MeshRenderer meshRenderer = hit.transform.GetComponent<MeshRenderer>();

                if (meshFilter != null && meshRenderer != null)
                {

                    DisplaceVertices a = hit.transform.gameObject.AddComponent<DisplaceVertices>();
                    a.displacementMap = displacementMap;
                    a.displacementShader = Instantiate(displacementShader);
                    a.displacementScale = 0.005f;
                }
            }
        }
    }
}
