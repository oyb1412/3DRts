using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class UIBase : MonoBehaviour
{
    private Dictionary<Type, Object[]> _objects = new Dictionary<Type, Object[]>();

    protected void Bind<T>(Type type) where T : Object
    {
        string[] names = Enum.GetNames(type);
        Object[] objects = new Object[names.Length];

        for (int i = 0; i < names.Length; i++)
        {
            objects[i] = Util.FindChild(gameObject, names[i]);
        }
        
        _objects.Add(typeof(T), objects);
    }

    protected GameObject Get<T>(int index) where T : Object
    {
        if (_objects.TryGetValue(typeof(T), out Object[] objects) == false)
            return null;

        return (GameObject)objects[index];
    }

    protected GameObject Get(int index)
    {
        if (_objects.TryGetValue(typeof(GameObject), out Object[] objects) == false)
            return null;

        return (GameObject)objects[index];
    }

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
