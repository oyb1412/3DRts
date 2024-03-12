using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class UIBase : MonoBehaviour
{
    protected Dictionary<Type, Object[]> _objects = new Dictionary<Type, Object[]>();
    public enum Images
    {
        Icon,
    }

    protected enum Buttons
    {
        
    }

    protected enum Texts
    {
        
    }
    
    
    private void Start()
    {
        
    }

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

    protected T Get<T>(int index) where T : Object
    {
        if (_objects.TryGetValue(typeof(T), out Object[] objects) == false)
            return null;

        return objects[index].GetComponent<T>();
    }

    protected void BindEvent(GameObject go, Action<PointerEventData> action, Define.MouseEventType type)
    {
        UI_EventHandler evt = Util.GetorAddComponent<UI_EventHandler>(go);

        switch (type)
        {
            case Define.MouseEventType.Enter:
                evt.OnEnterHandler += action;
                break;
            case Define.MouseEventType.LeftClick:
                evt.OnClickHandler += action;
                break;
        }
    }
    
    protected Image GetImage(int index) { return Get<Image>(index); }
    protected Button GetButton(int index) { return Get<Button>(index); }
    protected Text GetText(int index) { return Get<Text>(index); }
}
