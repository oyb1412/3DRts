using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IDragHandler
{
    public Action<PointerEventData> OnEnterHandler;
    public Action<PointerEventData> OnClickHandler;
    public Action<PointerEventData> OnDragHandler;
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickHandler?.Invoke(eventData);
        Debug.Log($"Click {eventData.GetType()}");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnEnterHandler?.Invoke(eventData);
        Debug.Log($"Enter {eventData.GetType()}");

    }

    public void OnDrag(PointerEventData eventData)
    {
        OnDragHandler?.Invoke(eventData);
        Debug.Log($"Drag {eventData.position}");
    }
}
