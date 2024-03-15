using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define 
{
    public enum BuildList
    {
        Castle,
        Barrack,
    }
    public enum State
    {
        None,
        Idle,
        Move,
        Attack,
        Die,
        Patrol,
        Hold,
        Build,
    }
    public enum MouseEventType
    {
        None,
        LeftClick,
        RightClick,
        Press,
        PressUp,
        PointDown,
        PointUp,
        Enter,
        Drag,
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
        Player,
    }

    public enum Cursor
    {
        Main,
        Attack,
        Patrol,
    }
}
