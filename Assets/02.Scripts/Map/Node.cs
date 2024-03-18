using System;
using UnityEngine;

[Flags]
public enum InteractableTypes
{
    None,
    Building,
    Mineral,
}

public enum RevealedTypes
{
    Black,
    Gray,
    White
}
public struct NodeData
{
    public InteractableTypes BnteractableTypes;
    public bool isTrigger;
    public bool HasBeenSeen;
    public int X;
    public int Z;
}

public class Node : MonoBehaviour
{
    public Terrain _terrain;
    public NodeData[,] Buildings;
    void Awake()
    {
        _terrain = GetComponent<Terrain>();
        Vector3 terrainSize = _terrain.terrainData.size;
        Buildings = new NodeData[(int)terrainSize.x, (int)terrainSize.z];
        for (int z = 0; z < Buildings.GetLength(0); z++)
        {
            for (int x = 0; x < Buildings.GetLength(1); x++)
            {
                Buildings[z, x].X = x;
                Buildings[z, x].Z = z;
            }
        }
    }
    
    public void SetNodeColor(int x, int z, Color color, bool trigger)
    {
        if (Buildings[x, z].isTrigger)
            return;
        
        Buildings[x, z].isTrigger = trigger;

    }
    
    public void UpdateNodesColor(Transform tr, float siteRange)
    {
        Color[] colors = Managers.Instance.MiniMap.MinimapTexture.GetPixels();
        
        bool isUpdate = false;
        for (int z = 0; z < Buildings.GetLength(0); z++)
        {
            for (int x = 0; x < Buildings.GetLength(1); x++)
            {
                float dis = (new Vector3(Buildings[z, x].X, 2f, Buildings[z, x].Z) - tr.position).magnitude;
                if (dis > siteRange)
                {
                    if(!Buildings[z, x].HasBeenSeen)
                        continue;
                    
                    colors[z * Buildings.GetLength(0) + x] = new Color(1f,0f,0f,0.5f);
                    isUpdate = true;
                }
                else 
                {
                    if(Buildings[z, x].isTrigger)
                        continue;
                    
                    Buildings[z, x].isTrigger = true;
                    Buildings[z, x].HasBeenSeen = true;
                    colors[z * Buildings.GetLength(0) + x] = Color.clear; 
                    isUpdate = true;
                }
            }
        }
        
        if(isUpdate)
            Managers.Instance.MiniMap.UpdateMinimap(colors);
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
        for (int z = 0; z < Buildings.GetLength(1); z++)
        {
            for (int x = 0; x < Buildings.GetLength(0); x++)
            {
                if (bound.Contains(Buildings[z,x].X, Buildings[z,x].Z))
                    Buildings[z, x].BnteractableTypes = InteractableTypes.Building;
            }
        }
    }
}
