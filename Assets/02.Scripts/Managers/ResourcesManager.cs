using System;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

public class ResourcesManager 
{
    public Object Load<T>(string path) where T : Object
    {
        if (typeof(T) != typeof(GameObject)) return Resources.Load<T>(path);
        string name = path;
        int index = name.LastIndexOf("/", StringComparison.Ordinal);
        if (index > 0)
            name = name.Substring(index + 1);

        GameObject go = Managers.Pool.GetOriginal(name);
        if (go != null)
            return go as T;

        return Resources.Load<T>(path);
    }

    public GameObject Activation(string path, Transform parent)
    {
        GameObject obj = Load<GameObject>($"Prefabs/{path}").GameObject();

        if (obj == null)
        {
            Debug.Log($"Failed Search Path : {path}");
            return null;
        }

        if (obj.GetComponent<Poolable>() != null)
            return Managers.Pool.Activation(obj).gameObject;
        
        GameObject go = Object.Instantiate(obj, parent);
        go.name = obj.name;
        return go;
    }
    
    

    public void Release(GameObject go)
    {
        if (go == null)
        {
            Debug.Log($"Failed Search GameObject : {go.name}");
            return;
        }

        Poolable poolable = go.GetComponent<Poolable>();
        if (poolable != null)
        {
            Managers.Pool.Release(poolable);
            return;
        }
        
        Object.Destroy(go);
    }
}
