using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager
{
    private Dictionary<Define.BuildList, GameObject> _building = new Dictionary<Define.BuildList, GameObject>();
    private GameObject currentBuilding;
    private Util.MyRect _bound;
    private MeshRenderer currentMesh;
    private Material _original;
    private Material _red;
    public void Init()
    {
        GameObject[] building = Resources.LoadAll<GameObject>($"Building");
        for(int i = 0;i < building.Length; i++)
        {
            _building.Add((Define.BuildList)i, building[i]);
        }

        _original = Resources.Load<Material>("Material/BuildingOiriginal");
        _red = Resources.Load<Material>("Material/BuildingRed");

        Managers.Input.OnMouseEvent += Create;
    }
    
    public void BuildShadow(Define.BuildList building)
    {
        currentBuilding = Managers.Resources.Activation($"Building/{Enum.GetName(typeof(Define.BuildList), building)}", null);
        currentMesh = currentBuilding.GetComponent<MeshRenderer>();
    }

    private void Create(Define.MouseEventType type)
    {
        if (!currentBuilding)
            return;

        switch (type)
        {
            //todo
            //esc로 설치 취소 구현
            case Define.MouseEventType.RightClick:
                currentBuilding = null;
                break;
            case Define.MouseEventType.LeftClick:
                //todo 
                //타워 설치
                break;
        }
    }
    
    public void OnUpdate()
    {
        if (!currentBuilding)
            return;
        
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.WorldToScreenPoint(currentBuilding.transform.position).z;

        Vector3 objectPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        currentBuilding.transform.position = new Vector3(objectPosition.x,2f,objectPosition.z);
        var ren = 
        _bound.minX = currentBuilding.transform.position.x - currentMesh.bounds.size.x / 2;
        _bound.maxX = currentBuilding.transform.position.x + currentMesh.bounds.size.x / 2;
        _bound.minZ = currentBuilding.transform.position.z - currentMesh.bounds.size.z / 2;
        _bound.maxZ = currentBuilding.transform.position.z + currentMesh.bounds.size.z / 2;

        var node = Managers.Instance.Node.Buildings;
        foreach (var item in node)
        {
            if (_bound.Contains(item.X, item.Z))
            {
                if (item.interactableTypes == InteractableTypes.Building)
                {
                    currentMesh.material = _red;
                    return;
                }
                if(item.interactableTypes == InteractableTypes.None)
                {
                    currentMesh.material = _original;
                }
            }
        }
        
    }
    
    
    
    public void Destory()
    {
        
    }
}
