using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Util : MonoBehaviour
{
    public static GameObject FindChild(GameObject go, string name) 
    {
        if (go == null || string.IsNullOrEmpty(name))
            return null;

        foreach (Transform child in go.transform)
        {
            if (child.name == name)
                return child.gameObject;
        }

        return null;
    }

    public static T GetorAddComponent<T>(GameObject go) where T : Component
    {
        var component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();

        return component;
    }

    public static PlayerController[] SelectedAutoUnits(PlayerController player,PlayerController.Type type, float range, int maxCount)
    {
        Collider[] units = new Collider[maxCount];
        int mask = (1 << (int)Define.Layer.Player);
        units = Physics.OverlapSphere(player.transform.position, range, mask);
        PlayerController[] players = new PlayerController[maxCount];
        for (int i = 0; i < units.Length; i++)
        {
            PlayerController unit = units[i].GetComponent<PlayerController>();
            if (unit.MyType == type)
                players[i] = unit;
            
            if (unit == null)
                return players;
        }

        return players;
    }

    public static GameObject SortToShotDistance(Collider[] obj, Transform pos)
    {
        if (obj.Length == 0)
            return null;
        
        for (int i = 0; i < obj.Length - 2; i++)
        {
            for (int j = 0; j < obj.Length - 1; j++)
            {
                float dis1 = (pos.transform.position - obj[i].transform.position).magnitude;
                float dis2 = (pos.transform.position - obj[i + 1].transform.position).magnitude;
                if (dis1 > dis2)
                {
                    (obj[i], obj[i + 1]) = (obj[i + 1], obj[i]);
                }
            }
        }

        return obj[0].transform.gameObject;
    }
        
}
