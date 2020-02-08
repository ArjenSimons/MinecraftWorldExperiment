using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class VoxelSystem : MonoBehaviour
{
    [SerializeField] private Vector3Int mapSize;
    [SerializeField] private float cellSize = 1;
   
    [SerializeField] [Range(2, 20)] private float frequency = 8;
    [SerializeField] private int amplitude;
    private byte[,,] map;

    public UnityEvent onSettingsChanged = new UnityEvent();

    public int Width => mapSize.x;
    public int Depth => mapSize.z;
    public int Height => mapSize.y;
    public float CellSize => cellSize;

    public byte GetCell(int x, int y, int z)
    {
        return map[x, y, z];
    }

    public byte GetNeigbor(int x, int y, int z, Direction dir)
    {
        Vector3Int dirOffset = directionOffsets[(int)dir];

        Vector3Int neighborPos = new Vector3Int(x, y, z) + dirOffset;

        if (CellIsInMap(neighborPos))
            return GetCell(neighborPos.x, neighborPos.y, neighborPos.z);

        return 0;
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

    public enum Block
    {
        AIR,
        GRASS,
        DIRT,
        STONE
    }

    private void Awake()
    {
        InitMap();
    }

    private void InitMap()
    {
        map = new byte[mapSize.x, mapSize.y, mapSize.z];

        if (amplitude > mapSize.y)
        {
            amplitude = mapSize.y;
        }

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int z = 0; z < mapSize.z; z++)
            {
                int height = (mapSize.y - (amplitude - 1)) + Mathf.RoundToInt(Mathf.PerlinNoise(x / frequency, z / frequency) * amplitude);

                for (int y = 0; y < mapSize.y; y++)
                {
                    byte byteCode = (int)Block.AIR;
                    if (y < height - 5)
                        byteCode = (int)Block.STONE;
                    else if (y < height - 1)
                        byteCode = (int)Block.DIRT;
                    else if (y < height)
                        byteCode = (int)Block.GRASS;

                    map[x, y, z] = byteCode;
                }
            }
        }
    }

    private void OnValidate()
    {
        InitMap();
        onSettingsChanged.Invoke();
    }
}
