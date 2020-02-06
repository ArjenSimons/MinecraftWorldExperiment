using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCube : GenerateMesh
{

    [Tooltip("The amount of vertices on the x, y and z axis")]
    [SerializeField] private Vector3Int gridSize;
    [SerializeField] private float cellSize = 1;

    public override Mesh Invoke()
    {
        base.Invoke();

        CalculateVertices();
        StartCoroutine(CalculateFaces());

        SetMesh();

        return mesh;
    }

    public void CreatePlane(Vector3 position, Quaternion rotation)
    {

    }

    public void CalculateVertices()
    {
        int middleVertices = (gridSize.x - 2) * (gridSize.y - 2) * (gridSize.z - 2);

        vertices = new Vector3[gridSize.x * gridSize.y * gridSize.z - middleVertices];

        int iVertex = 0;
        for (int y = 0; y < gridSize.y; y++)
        {
            //-z
            for (int x = 0; x < gridSize.x; x++, iVertex++)
            {
                vertices[iVertex] = new Vector3(
                    x * cellSize - (gridSize.x - 1) * cellSize / 2,
                    y * cellSize - (gridSize.y - 1) * cellSize / 2,
                    -(gridSize.z - 1) * cellSize / 2);
            }
            //+x
            for (int z = 1; z < gridSize.z; z++, iVertex++)
            {
                vertices[iVertex] = new Vector3(
                    (gridSize.x - 1) * cellSize / 2,
                    y * cellSize - (gridSize.y - 1) * cellSize / 2,
                    z * cellSize - (gridSize.z - 1) * cellSize / 2);
            }

            //+z
            for (int x = gridSize.x - 2; x > -1; x--, iVertex++)
            {
                vertices[iVertex] = new Vector3(
                    x * cellSize - (gridSize.x - 1) * cellSize / 2,
                    y * cellSize - (gridSize.y - 1) * cellSize / 2,
                    (gridSize.z - 1) * cellSize / 2);
            }

            //-x
            for (int z = gridSize.z - 2; z > 0; z--, iVertex++)
            {
                vertices[iVertex] = new Vector3(
                    0 * cellSize - (gridSize.x - 1) * cellSize / 2,
                    y * cellSize - (gridSize.y - 1) * cellSize / 2,
                    z * cellSize - (gridSize.z - 1) * cellSize / 2);
            }
        }

        //Top
        for (int z = 1; z < gridSize.z - 1; z++)
        {
            for (int x = 1; x < gridSize.x - 1; x++, iVertex++)
            {
                vertices[iVertex] = new Vector3(
                    x * cellSize - (gridSize.x - 1) * cellSize / 2,
                    (gridSize.y - 1) * cellSize / 2,
                    z * cellSize - (gridSize.z - 1) * cellSize / 2);
            }
        }

        //Bottom
        for (int z = 1; z < gridSize.z - 1; z++)
        {
            for (int x = 1; x < gridSize.x - 1; x++, iVertex++)
            {
                vertices[iVertex] = new Vector3(
                    x * cellSize - (gridSize.x - 1) * cellSize / 2,
                    -(gridSize.y - 1) * cellSize / 2,
                    z * cellSize - (gridSize.z - 1) * cellSize / 2);
            }
        }
    }

    public IEnumerator CalculateFaces()
    {
        triangles = new int[12 * (((gridSize.x - 1) * (gridSize.y - 1)) + ((gridSize.x - 1) * (gridSize.z - 1)) + ((gridSize.y - 1) * (gridSize.z - 1)))];

        int ringSize = (gridSize.x - 1 + gridSize.z - 1) * 2;
        int vertex = 0, triangle = 0;
        //Fill Sides
        for (int y = 0; y < gridSize.y - 1; y++, vertex++, triangle += 6)
        {
            for (int ring = 0; ring < ringSize - 1; ring++, vertex++, triangle += 6)
            {
                SetQuad(triangle, vertex, vertex + ringSize, vertex + 1, vertex + ringSize + 1);
                yield return new WaitForSeconds(0.1f);
                SetMesh();
            }
            SetQuad(triangle, vertex, vertex + ringSize, vertex - ringSize + 1, vertex + 1);
            yield return new WaitForSeconds(0.1f);
            SetMesh();
        }

        //Fill Top
        StartCoroutine(CalculateTopFaces(ringSize, vertex, triangle));
    }

    public IEnumerator CalculateTopFaces(int ringSize, int vertex, int triangle)
    {
        int firstNode = (gridSize.y - 1) * ringSize;
        Debug.Log("firstNode: " + firstNode);
        for (int x = 0; x < gridSize.x - 2; x++, vertex++, triangle += 6)
        {
            SetQuad(triangle, x + firstNode, x + firstNode + ringSize - 1, x + firstNode + 1, x + firstNode + ringSize);
            yield return new WaitForSeconds(1f);
            SetMesh();
        }
        SetQuad(triangle, firstNode + gridSize.x - 2, firstNode + ringSize - 2, firstNode + gridSize.x - 1, firstNode + ringSize + gridSize.x - 3);
        vertex++;
        triangle += 6;
        yield return new WaitForSeconds(0.1f);
        SetMesh();
    }

    public void SetQuad(int i, int v00, int v10, int v01, int v11)
    {
        Debug.Log("i: " + i + "   v00: " + v00 + "   v10:  " + v10 + "   v01: " + v01 + "   v11: " + v11);
        triangles[i] = v00;
        triangles[i + 1] = v10;
        triangles[i + 2] = v01;
        triangles[i + 3] = v10;
        triangles[i + 4] = v11;
        triangles[i + 5] = v01;
    }
}
