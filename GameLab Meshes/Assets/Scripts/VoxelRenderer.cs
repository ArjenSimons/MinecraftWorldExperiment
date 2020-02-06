using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VoxelRenderer : MonoBehaviour
{
    [SerializeField] private VoxelSystem voxel;

    private Mesh mesh;
    private List<Vector3Int> vertices;
    private List<int> triangles;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    private void Start()
    {
        GenerateVoxelMesh();
    }

    private void GenerateVoxelMesh()
    {
        vertices = new List<Vector3Int>();
        triangles = new List<int>();

        for (int x = 0; x < voxel.Width; x++)
        {
            for (int z = 0; z < voxel.Depth; z++)
            {
                for (int y = 0; y < voxel.Height; y++)
                {
                    if (voxel.GetCell(x, y, z) == 0)
                        continue;

                    MakeCube(voxel.CellSize, new Vector3Int(x, y, z));
                }
            }
        }
    }

    private void MakeCube(float scale, Vector3Int position)
    {
        for (int i = 0; i < 6; i++)
        {
            MakeFace(i, scale, position);
        }
    }

    private void MakeFace(int dir, float scale, Vector3Int position)
    {
        vertices.AddRange(GetFaceVertices(dir, scale, position));
    }

    private Vector3Int[] GetFaceVertices(int dir, float Scale, Vector3Int position)
    {
        //Vector3Int[] vertices =
        //{
        //    new Vector3Int()
        //}

        return new Vector3Int[] { };
        
    }
}
