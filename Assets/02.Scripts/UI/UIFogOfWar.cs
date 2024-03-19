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
        Bind<RawImage>(typeof(RawImages));

        for (int i = 0; i < Enum.GetValues(typeof(RawImages)).Length; i++)
        {
            RawImage.Add(Get<RawImage>(i).GetComponent<RawImage>());
        }
        RawImage[(int)RawImages.FogOfWarRawImage].texture = Texture;
    }
}
