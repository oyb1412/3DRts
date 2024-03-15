using System;
using System.Collections;
using System.Collections.Generic;
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
    public InteractableTypes interactableTypes;
    public int X;
    public int Z;
}

public class Node : MonoBehaviour
{
    Terrain terrain;
    public NodeData[,] Buildings;
    void Start()
    {
        terrain = GetComponent<Terrain>();
        Vector3 terrainSize = terrain.terrainData.size;
        Buildings = new NodeData[(int)terrainSize.x, (int)terrainSize.z];
        for (int z = 0; z < Buildings.GetLength(1); z++)
        {
            for (int x = 0; x < Buildings.GetLength(0); x++)
            {
                Buildings[z, x].X = x;
                Buildings[z, x].Z = z;
            }
        }

        Buildings[1, 1].interactableTypes = InteractableTypes.Building;
    }
}
