using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(VoxelSystem))]
public class VoxelRenderer : MonoBehaviour
{
    private VoxelSystem voxel;

    private Mesh mesh;
    private List<Vector3> vertices;
    private List<int> triangles;

    private float adjustedScale;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        voxel = GetComponent<VoxelSystem>();
        voxel.onSettingsChanged.AddListener(GenerateVoxelMesh);
    }

    private void Start()
    {
        GenerateVoxelMesh();
    }

    private void GenerateVoxelMesh()
    {
        adjustedScale = voxel.CellSize / 2;

        vertices = new List<Vector3>();
        triangles = new List<int>();

        for (int x = 0; x < voxel.Width; x++)
        {
            for (int z = 0; z < voxel.Depth; z++)
            {
                for (int y = 0; y < voxel.Height; y++)
                {
                    if (voxel.GetCell(x, y, z) == 0)
                        continue;   
                    //Debug.Log("x: " + x + "y: " + y + "z: " + z);
                    MakeCube(new Vector3Int(x, y, z));
                }
            }
        }

        SetMesh();
    }

    private void MakeCube(Vector3Int position)
    {
        for (int i = 0; i < 6; i++)
        {
            if (voxel.GetNeigbor(position.x, position.y, position.z, (VoxelSystem.Direction)i) == 0)
                MakeFace(i, position);
        }
    }

    private void MakeFace(int dir, Vector3 position)
    {
        int nVertices = vertices.Count;
        vertices.AddRange(GetFaceVertices(dir, position));

        triangles.Add(nVertices);
        triangles.Add(nVertices + 2);
        triangles.Add(nVertices + 1);
        triangles.Add(nVertices + 2);
        triangles.Add(nVertices + 3);
        triangles.Add(nVertices + 1);
    }

    private Vector3[] GetFaceVertices(int dir, Vector3 position)
    {
        Vector3[] faceVertices =
        {
            normalizedVertices[(int)quads[dir].x] * adjustedScale + position * voxel.CellSize,
            normalizedVertices[(int)quads[dir].y] * adjustedScale + position * voxel.CellSize,
            normalizedVertices[(int)quads[dir].z] * adjustedScale + position * voxel.CellSize,
            normalizedVertices[(int)quads[dir].w] * adjustedScale + position * voxel.CellSize
        };
        return faceVertices;        
    }

    private readonly Vector3[] normalizedVertices =
    {
        new Vector3(-1, -1, 1),
        new Vector3(1, -1, 1),
        new Vector3(-1, 1, 1),
        new Vector3(1, 1, 1),
        new Vector3(-1, -1, -1),
        new Vector3(1, -1, -1),
        new Vector3(-1, 1, -1),
        new Vector3(1, 1, -1)
    };


    private readonly Vector4[] quads =
    {
        new Vector4(3, 2, 7, 6),    //Up
        new Vector4(5, 4, 1, 0),    //Down
        new Vector4(1, 0, 3, 2),    //North
        new Vector4(4, 5, 6, 7),    //South
        new Vector4(5, 1, 7, 3),    //East
        new Vector4(0, 4, 2, 6)     //West
    };

    private void SetMesh()
    {
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateNormals();
    }
}
