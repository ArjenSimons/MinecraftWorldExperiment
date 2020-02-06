using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePlane : GenerateMesh
{
    [Tooltip("The amount of vertices on the x an y axis")]
    [SerializeField] private Vector2Int gridSize;

    [SerializeField] private float cellSize = 1;

    public override Mesh Invoke()
    {
        base.Invoke();

        //Place vertices
        vertices = new Vector3[gridSize.x * gridSize.y];
        uv = new Vector2[vertices.Length];
        tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0, 0, -1);

        for (int i = 0, y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++, i++)
            {
                vertices[i] = new Vector3(x * cellSize, y * cellSize);
                uv[i] = new Vector2(x / (cellSize * (gridSize.x - 1)), y / (cellSize * (gridSize.y - 1)));
                tangents[i] = tangent;
            }
        }

        //Draw faces
        triangles = new int[(gridSize.x - 1) * (gridSize.y - 1) * 6];

        for (int vertex = 0, triangle = 0, y = 0; y < gridSize.y - 1; y++, vertex++)
        {
            for (int x = 0; x < gridSize.x - 1; x++, vertex++, triangle += 6)
            {
                triangles[triangle] = vertex;
                triangles[1 + triangle] = vertex + gridSize.x;
                triangles[2 + triangle] = vertex + 1;


                triangles[3 + triangle] = vertex + 1;
                triangles[4 + triangle] = vertex + gridSize.x;
                triangles[5 + triangle] = vertex + gridSize.x + 1;
            }
        }

        SetMesh();
        return mesh;
    }
}
