using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFogOfWar : UIMap
{
    public enum RawImages
    {
        FogOfWarRawImage,
    }
   
    protected override void Start()
    {
        base.Start();
        RawImage.Add(Util.FindChild(gameObject, "FogOfWarRawImage").GetComponent<RawImage>());

        RawImage[(int)RawImages.FogOfWarRawImage].texture = Texture;
    }
}
