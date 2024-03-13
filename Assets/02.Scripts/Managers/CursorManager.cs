using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CursorManager
{
    private List<Texture2D> _cursors = new List<Texture2D>(5);
    public void Init()
    {
        Texture2D mainCursor = Resources.Load<Texture2D>("Texture/Cursors/Main");
        _cursors.Add(mainCursor);
        Texture2D attackCursor = Resources.Load<Texture2D>("Texture/Cursors/Attack");
        _cursors.Add(attackCursor);  
        Texture2D patrolCursor = Resources.Load<Texture2D>("Texture/Cursors/Patrol");
        _cursors.Add(patrolCursor);
        
        Cursor.SetCursor(mainCursor,Vector2.zero,CursorMode.Auto);
    }

    public void SetCursor(Define.Cursor type)
    {
        Cursor.SetCursor(_cursors[(int)type],Vector2.zero,CursorMode.Auto);
    }
}
