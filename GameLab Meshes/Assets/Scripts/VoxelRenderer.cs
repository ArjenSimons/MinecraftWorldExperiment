using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VoxelRenderer : MonoBehaviour
{
    [SerializeField] private VoxelSystem voxel;

    private Mesh mesh;
    private List<Vector3> vertices;
    private List<int> triangles;

    private float adjustedScale;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    private void Start()
    {
        GenerateVoxelMesh();

        adjustedScale = voxel.CellSize / 2;
        Debug.Log(adjustedScale);

        //Vector3 pos = new Vector3(1, 0, 0) * 0.5f;

        //pos = new Vector3(pos.x + -1, pos.y + 1, pos.z);

        //Debug.Log("x: " + pos.x);
        //Debug.Log("y: " + pos.y);
        //Debug.Log("z: " + pos.z);
    }

    private void GenerateVoxelMesh()
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();

        for (int x = 0; x < voxel.Width; x++)
        {
            for (int z = 0; z < voxel.Depth; z++)
            {
                for (int y = 0; y < voxel.Height; y++)
                {
                    Debug.Log(voxel.GetCell(x, y, z));
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
            //Debug.Log(voxel.GetNeigbor(position.x, position.y, position.z, (VoxelSystem.Direction)i));
            if (voxel.GetNeigbor(position.x, position.y, position.z, (VoxelSystem.Direction)i) != 0)
                MakeFace(i, position);
        }
    }

    private void MakeFace(int dir, Vector3Int position)
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

    private Vector3[] GetFaceVertices(int dir, Vector3Int position)
    {
        adjustedScale = 0.5f;
        Vector3 facePosition = (Vector3)voxel.directionOffsets[dir] * adjustedScale;
        //Debug.Log(facePosition.ToString());

        facePosition = new Vector3(facePosition.x + position.x, facePosition.y + position.y, facePosition.z + position.z);
        //  Debug.Log(position.ToString());
        //Debug.Log(adjustedScale);
        Vector3[] faceVertices =
        {
            //Bottom Left
            new Vector3(
                facePosition.x - adjustedScale * DirectionMultiplyAdjuster(voxel.directionOffsets[dir].x),
                facePosition.y - adjustedScale * DirectionMultiplyAdjuster(voxel.directionOffsets[dir].y),
                facePosition.z - adjustedScale * DirectionMultiplyAdjuster(voxel.directionOffsets[dir].z)),

            //BottomRight
            new Vector3(
                facePosition.x + adjustedScale * DirectionMultiplyAdjuster(voxel.directionOffsets[dir].x),
                facePosition.y - adjustedScale * DirectionMultiplyAdjuster(voxel.directionOffsets[dir].y),
                facePosition.z + adjustedScale * DirectionMultiplyAdjuster(voxel.directionOffsets[dir].z)),

            //TopLeft
            new Vector3(
                facePosition.x - adjustedScale * DirectionMultiplyAdjuster(voxel.directionOffsets[dir].x),
                facePosition.y + adjustedScale * DirectionMultiplyAdjuster(voxel.directionOffsets[dir].y),
                facePosition.z - adjustedScale * DirectionMultiplyAdjuster(voxel.directionOffsets[dir].z)),

            //TopRight
            new Vector3(
                facePosition.x + adjustedScale * DirectionMultiplyAdjuster(voxel.directionOffsets[dir].x),
                facePosition.y + adjustedScale * DirectionMultiplyAdjuster(voxel.directionOffsets[dir].y),
                facePosition.z + adjustedScale * DirectionMultiplyAdjuster(voxel.directionOffsets[dir].z))
        };
        return faceVertices;        
    }

    private void SetMesh()
    {
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateNormals();
    }

    private int DirectionMultiplyAdjuster(int input)
    {
        if (input != 0)
            return 0;
        else return 1;

       // throw new IndexOutOfRangeException("input must be number1 or number2 but was: " + input);
    }
}
