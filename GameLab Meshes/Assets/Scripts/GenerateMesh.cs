using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GenerateMesh : MonoBehaviour
{
    protected Mesh mesh;      

    protected Vector3[] vertices;
    protected int[] triangles;
    protected Vector2[] uv;
    protected Vector4[] tangents;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;

        Invoke();
    }

    virtual public Mesh Invoke()
    {
       mesh.Clear();

       return mesh;
    }

    protected void SetMesh()
    {
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.tangents = tangents;

        mesh.Optimize();
        mesh.RecalculateNormals();
    }

    private void OnValidate()
    {
        if(mesh != null)
            Invoke();
    }

    private void OnDrawGizmos()
    {
        if (vertices != null)
        {
            Gizmos.color = Color.black;
            for (int i = 0; i < vertices.Length; i++)
            {
                Gizmos.DrawSphere(transform.TransformPoint(vertices[i]), 0.1f);
            }
        }
    }
}
