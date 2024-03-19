using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMap : UIBase
{
    [HideInInspector]public List<RawImage> RawImage = new List<RawImage>();
    [HideInInspector]public Texture2D Texture;

    protected virtual void Start()
    {
        var node = Managers.Instance.Node;
        Texture = new Texture2D(node.Nodes.GetLength(0), node.Nodes.GetLength(1));
    }

    public void UpdateMap(Color[] color)
    {
        Texture.SetPixels(color);
        Texture.Apply(false);
    }
}
