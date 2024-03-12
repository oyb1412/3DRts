using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define 
{
    public enum MouseEventType
    {
        None,
        LeftClick,
        RightClick,
        Press,
        PointDown,
        PointUp,
        Enter,
    }

    public enum SceneType
    {
        None,
        InGame,
    }

    public enum Layer
    {
        Ground = 6,
        Monster,
    }
}
