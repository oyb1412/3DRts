using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public Action OnKeyboardEvent;
    public Action<Define.MouseEventType> OnMouseEvent;
    
    public void OnUpdate()
    {
        if (Input.anyKey)
        {
            OnKeyboardEvent?.Invoke();
        }

        if (Input.GetMouseButtonDown(0))
        {
            OnMouseEvent?.Invoke(Define.MouseEventType.LeftClick);
        } 
        
        if (Input.GetMouseButtonDown(1))
        {
            OnMouseEvent?.Invoke(Define.MouseEventType.RightClick);
        }
        
    }
}
