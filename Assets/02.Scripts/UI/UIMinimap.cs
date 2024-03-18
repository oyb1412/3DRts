using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMinimap : UIBase
{
    public enum RawImages
    {
        MinimapRawImage,
    }
   
    private List<RawImage> _rawImages = new List<RawImage>();
    public Texture2D MinimapTexture;
    private Color[] clearMap;
    private void Start()
    {
        Bind<RawImage>(typeof(RawImages));

        for (int i = 0; i < Enum.GetValues(typeof(RawImages)).Length; i++)
        {
            _rawImages.Add(Get<RawImage>(i).GetComponent<RawImage>());
        }

        var node = Managers.Instance.Node;
        MinimapTexture = new Texture2D(node.Buildings.GetLength(0), node.Buildings.GetLength(1));
        clearMap = new Color[node.Buildings.GetLength(0) * node.Buildings.GetLength(1)];
        for (int i = 0; i < clearMap.Length; i++)
        {
           clearMap[i] = Color.black;
        }
        MinimapTexture.SetPixels(clearMap);
        MinimapTexture.Apply();

        _rawImages[(int)RawImages.MinimapRawImage].texture = MinimapTexture;
    }

    //todo
    //한번이라도 밝힌적이 있나 없나 trigger지정 
    //유닛 주변이 아닌곳은 if문으로 trigger체크후, false면 블랙, true면 그레이로 지정
    public void UpdateMinimap(Color[] color)
    {
        MinimapTexture.SetPixels(color);
        MinimapTexture.Apply(false);
    }
}
