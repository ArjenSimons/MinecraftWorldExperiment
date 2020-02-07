using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelSystem : MonoBehaviour
{
    [SerializeField] private Vector3Int mapSize;
    [SerializeField] private float _cellSize;
    //[SerializeField] private GameObject cube;
   
    [SerializeField] [Range(2, 20)] private float frequency = 8;
    private byte[,,] map;

    public int Width => mapSize.x;
    public int Depth => mapSize.z;
    public int Height => mapSize.y;
    public float CellSize => _cellSize;

    public byte GetCell(int x, int y, int z)
    {
        return map[x, y, z];
    }

    public byte GetNeigbor(int x, int y, int z, Direction dir)
    {
        Vector3Int dirOffset = directionOffsets[(int)dir];

        Vector3Int neighborPos = new Vector3Int(x + dirOffset.x, y + dirOffset.y, z + dirOffset.z);

        if (!CellIsInMap(neighborPos))
            return 1;

        return GetCell(neighborPos.x, neighborPos.y, neighborPos.z);
    }

    private bool CellIsInMap(Vector3Int position)
    {
        if (position.x < 0 ||
            position.x > mapSize.x - 1 ||
            position.y < 0 ||
            position.y > mapSize.y - 1 ||
            position.z < 0 ||
            position.z > mapSize.z - 1)
        {
            return false;
        }
        return true;
    }

    public readonly Vector3Int[] directionOffsets =
    {
        new Vector3Int(0, 1, 0),    //Up
        new Vector3Int(0, -1, 0),   //Down
        new Vector3Int(0, 0, 1),    //North
        new Vector3Int(0, 0, -1),   //South
        new Vector3Int(1, 0, 0),    //East
        new Vector3Int(-1, 0, 0)    //West
    };

    public enum Direction
    {
        UP,     // +y
        DOWN,   // -y
        NORTH,  // +z
        SOUTH,  // -z
        EAST,   // +x
        WEST    // -x
    }

    private void Awake()
    {
        InitMap();


    }

    private void InitMap()
    {
        map = new byte[mapSize.x, mapSize.y, mapSize.z];

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int z = 0; z < mapSize.z; z++)
            {
                int height = Mathf.RoundToInt(Mathf.PerlinNoise(x / frequency, z / frequency) * mapSize.y);
                Debug.Log(height);
                for (int y = 0; y < mapSize.y; y++)
                {
                    byte byteCode = 0;
                    if (y < height)
                        byteCode = 1;

                    map[x, y, z] = byteCode;

                    if (byteCode == 1)
                    {
                      //  Instantiate(cube, new Vector3(x, y, z), Quaternion.identity, transform);
                    }
                }
            }
        }

    }
}
