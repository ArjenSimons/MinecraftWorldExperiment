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

        //Quaternion rotationOffset = Quaternion.Euler(0, 0, 180);

        //Matrix4x4 rotation = Matrix4x4.Rotate(Quaternion.Euler(0, 0, 90));

        //Debug.Log(rotation.ToString());

        //Vector3 vertexPos = rotationOffset * (new Vector3(10 - adjustedScale, 10 - adjustedScale, 10 - adjustedScale) - new Vector3(10, 10, 10)) + new Vector3(10, 10, 10);
        ////Vector3 vertexPos = rotationOffset * (new Vector3(10 - adjustedScale, 10 - adjustedScale, 10 - adjustedScale) - new Vector3(10, 10, 10)) + new Vector3(10, 10, 10);
        //Debug.Log(vertexPos.ToString());

        //Vector3 pos = new Vector3(1, 0, 0) * 0.5f;
        //Vector3 pos =  Quaternion.Euler(90, 0, 00) * Vector3.up;

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
        Debug.Log(adjustedScale);
        adjustedScale = 0.5f;
        Vector3[] faceVertices =
        {
            normalizedVertices[(int)quads[dir].x] * adjustedScale + position,
            normalizedVertices[(int)quads[dir].y] * adjustedScale + position,
            normalizedVertices[(int)quads[dir].z] * adjustedScale + position,
            normalizedVertices[(int)quads[dir].w] * adjustedScale + position
        };
        //Vector3 direction = (Vector3)voxel.directionOffsets[dir];
        //adjustedScale = 0.5f;
        //Vector3 facePosition = (Vector3)voxel.directionOffsets[dir] * adjustedScale;
        ////Debug.Log(facePosition.ToString());

        //facePosition = new Vector3(facePosition.x + position.x, facePosition.y + position.y, facePosition.z + position.z);
        ////Debug.Log(position.ToString());
        ////Debug.Log(adjustedScale);
        //Quaternion rotationOffset =  Quaternion.Euler(direction.x * 90, direction.y * 90, direction.z * 90);
        //Vector3[] faceVertices =
        //{
        //    rotationOffset * (direction - new Vector3(position.x - adjustedScale, position.y- adjustedScale, position.z)) + direction
        //    //rotationOffset * new Vector3(position.x - adjustedScale, position.y - adjustedScale, -adjustedScale),
        //    //rotationOffset * new Vector3(position.x + adjustedScale, position.y - adjustedScale, -adjustedScale),
        //    //rotationOffset * new Vector3(position.x - adjustedScale, position.y + adjustedScale, -adjustedScale),
        //    //rotationOffset * new Vector3(position.x + adjustedScale, position.y + adjustedScale, -adjustedScale)
        //};
        //Vector3[] faceVertices =
        //{
        //    //Bottom Left
        //    new Vector3(
        //        facePosition.x - adjustedScale * DirectionMultiplyAdjuster(voxel.directionOffsets[dir].x),
        //        facePosition.y - adjustedScale * DirectionMultiplyAdjuster(voxel.directionOffsets[dir].y),
        //        facePosition.z - adjustedScale * DirectionMultiplyAdjuster(voxel.directionOffsets[dir].z)),

        //    //BottomRight
        //    new Vector3(
        //        facePosition.x + adjustedScale * DirectionMultiplyAdjuster(voxel.directionOffsets[dir].x),
        //        facePosition.y - adjustedScale * DirectionMultiplyAdjuster(voxel.directionOffsets[dir].y),
        //        facePosition.z + adjustedScale * DirectionMultiplyAdjuster(voxel.directionOffsets[dir].z)),

        //    //TopLeft
        //    new Vector3(
        //        facePosition.x - adjustedScale * DirectionMultiplyAdjuster(voxel.directionOffsets[dir].x),
        //        facePosition.y + adjustedScale * DirectionMultiplyAdjuster(voxel.directionOffsets[dir].y),
        //        facePosition.z - adjustedScale * DirectionMultiplyAdjuster(voxel.directionOffsets[dir].z)),

        //    //TopRight
        //    new Vector3(
        //        facePosition.x + adjustedScale * DirectionMultiplyAdjuster(voxel.directionOffsets[dir].x),
        //        facePosition.y + adjustedScale * DirectionMultiplyAdjuster(voxel.directionOffsets[dir].y),
        //        facePosition.z + adjustedScale * DirectionMultiplyAdjuster(voxel.directionOffsets[dir].z))
        //};
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


    private readonly Vector4[] quads = ///1!!!!!!!!!!!!!!! Check order (face drawn direction)
    {
        new Vector4(3, 2, 7, 6),    //Up
        new Vector4(5, 4, 1, 0),    //Down
        new Vector4(7, 6, 5, 4),    //North
        new Vector4(1, 2, 3, 2),    //South
        new Vector4(5, 1, 7, 3),    //East
        new Vector4(0, 4, 2, 6)     //West
    };

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
