using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class UIBase : MonoBehaviour
{
    protected void BindEvent(GameObject go, Action<PointerEventData> action, Define.MouseEventType type)
    {
        UIEventHandler evt = Util.GetOrAddComponent<UIEventHandler>(go);

        switch (type)
        {
            case Define.MouseEventType.Enter:
                evt.OnEnterHandler += action;
                break;
            case Define.MouseEventType.LeftClick:
                evt.OnClickHandler += action;
                break;
            case Define.MouseEventType.Drag:
            evt.OnDragHandler += action;
                break;
        }
    }
}
