using System;
using UnityEngine;
using UnityEngine.Serialization;

[Flags]
public enum NodeTypes
{
    None,
    Building,
    Unit,
}

public struct NodeData
{
    public NodeTypes NodeTypes;
    public bool HasFoundSeen;
    public bool HasBeenSeen;
    public int PosX;
    public int PosZ;
}

public class Node : MonoBehaviour
{
    public Terrain Terrain;
    public NodeData[,] Nodes;
    void Awake()
    {
        Terrain = GetComponent<Terrain>();
        Vector3 terrainSize = Terrain.terrainData.size;
        Nodes = new NodeData[(int)terrainSize.x, (int)terrainSize.z];
        for (int z = 0; z < Nodes.GetLength(0); z++)
        {
            for (int x = 0; x < Nodes.GetLength(1); x++)
            {
                Nodes[z, x].PosX = x;
                Nodes[z, x].PosZ = z;
            }
        }
    }

    private void Update()
    {
        var units = GameObject.FindObjectsByType<Mark>(FindObjectsSortMode.None);
        ResetNodeTriggers();
        foreach (var unit in units)
        {
            CheckUnitSightRange(unit.transform, 10f);
        }
        UpdateNodeColors();
    }
    
    void CheckUnitSightRange(Transform unitTransform, float sightRange)
    {
        for (int z = 0; z < Nodes.GetLength(0); z++)
        {
            for (int x = 0; x < Nodes.GetLength(1); x++)
            {
                float distance = (new Vector3(Nodes[z, x].PosX, 0, Nodes[z, x].PosZ) - unitTransform.position).magnitude;
                if (distance < sightRange)
                {
                    Nodes[z, x].HasFoundSeen = true; 
                    Nodes[z, x].HasBeenSeen = true; 
                }
                
                float unitDistance = (new Vector3(Nodes[z, x].PosX, 0, Nodes[z, x].PosZ) - unitTransform.position).magnitude;
                if (unitDistance < 3f)
                {
                    Nodes[z, x].NodeTypes = NodeTypes.Unit;
                }

            }
        }
    }
    
    void UpdateNodeColors()
    {
        Color[] miniMapColors = Managers.Instance.MiniMap.Texture.GetPixels();
        Color[] fogOfWarColors = Managers.Instance.FogOfWar.Texture.GetPixels();
        bool isUpdate = false;

        for (int z = 0; z < Nodes.GetLength(0); z++)
        {
            for (int x = 0; x < Nodes.GetLength(1); x++)
            {
                if (Nodes[z, x].HasBeenSeen && !Nodes[z, x].HasFoundSeen)
                {
                    miniMapColors[z * Nodes.GetLength(0) + x] = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                    fogOfWarColors[(z) * Nodes.GetLength(0) + x] = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                    isUpdate = true;
                }
                else if (Nodes[z, x].HasFoundSeen)
                {
                    if (Nodes[z, x].NodeTypes == NodeTypes.Unit ||
                        Nodes[z, x].NodeTypes == NodeTypes.Building)
                    {
                        miniMapColors[z * Nodes.GetLength(0) + x] = new Color(0f, 1f, 0f, 0.5f);
                        fogOfWarColors[z * Nodes.GetLength(0) + x] = Color.clear;
                        isUpdate = true;
                    }
                    else
                    {
                        miniMapColors[z * Nodes.GetLength(0) + x] = Color.clear;
                        fogOfWarColors[z * Nodes.GetLength(0) + x] = Color.clear;
                        isUpdate = true;
                    }
                   
                }
                else
                {
                    miniMapColors[z * Nodes.GetLength(0) + x] = Color.black;
                    fogOfWarColors[z * Nodes.GetLength(0) + x] = Color.black;
                    isUpdate = true;
                }
            }
        }
        if (isUpdate)
        {
            Managers.Instance.MiniMap.UpdateMap(miniMapColors);
            Managers.Instance.FogOfWar.UpdateMap(fogOfWarColors);
        }
    }
    void ResetNodeTriggers()
    {
        for (int z = 0; z < Nodes.GetLength(0); z++)
        {
            for (int x = 0; x < Nodes.GetLength(1); x++)
            {
                Nodes[z, x].HasFoundSeen = false;
                
                if(Nodes[z,x].NodeTypes != NodeTypes.Building)
                    Nodes[z, x].NodeTypes = NodeTypes.None;

            }
        }
    }
    
    public void SetNode(GameObject go)
    {
        Util.MyRect bound;
        MeshRenderer currentMesh = go.GetComponent<MeshRenderer>();
        bound.MinX = go.transform.position.x - currentMesh.bounds.size.x / 2;
        bound.MaxX = go.transform.position.x + currentMesh.bounds.size.x / 2;
        bound.MinZ = go.transform.position.z - currentMesh.bounds.size.z / 2;
        bound.MaxZ = go.transform.position.z + currentMesh.bounds.size.z / 2;
        SetNode(bound);
    }

    public void SetNode(Util.MyRect bound)
    {
        for (int z = 0; z < Nodes.GetLength(1); z++)
        {
            for (int x = 0; x < Nodes.GetLength(0); x++)
            {
                if (bound.Contains(Nodes[z,x].PosX, Nodes[z,x].PosZ))
                    Nodes[z, x].NodeTypes = NodeTypes.Building;
            }
        }
    }
}
