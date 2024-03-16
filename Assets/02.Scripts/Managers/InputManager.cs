using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action OnKeyboardEvent;
    public Action<Define.MouseEventType> OnMouseEvent;
    
    public void OnUpdate()
    {
        if (Input.GetMouseButtonUp(0))
        {
            OnMouseEvent?.Invoke(Define.MouseEventType.PressUp);
        }

        if (EventSystem.current.IsPointerOverGameObject())
            return;
        

        if (Input.anyKey)
        {
            OnKeyboardEvent?.Invoke();
        }
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseEvent?.Invoke(Define.MouseEventType.LeftClick);
        } 
        if (Input.GetMouseButton(0))
        {
            OnMouseEvent?.Invoke(Define.MouseEventType.Press);
        } 
        if (Input.GetMouseButtonDown(1))
        {
            OnMouseEvent?.Invoke(Define.MouseEventType.RightClick);
        }

  
        
    }
}
