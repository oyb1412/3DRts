using System;
using UnityEngine;

[Flags]
public enum InteractableTypes
{
    None,
    Building,
    Mineral,
}
public struct NodeData
{
    public InteractableTypes BnteractableTypes;
    public int X;
    public int Z;
}

public class Node : MonoBehaviour
{
    Terrain _terrain;
    public NodeData[,] Buildings;
    void Start()
    {
        _terrain = GetComponent<Terrain>();
        Vector3 terrainSize = _terrain.terrainData.size;
        Buildings = new NodeData[(int)terrainSize.x, (int)terrainSize.z];
        for (int z = 0; z < Buildings.GetLength(1); z++)
        {
            for (int x = 0; x < Buildings.GetLength(0); x++)
            {
                Buildings[z, x].X = x;
                Buildings[z, x].Z = z;
            }
        }

        Buildings[1, 1].BnteractableTypes = InteractableTypes.Building;
    }
}
