using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager
{
    private bool _canBuild;
    private List<GameObject> _building = new List<GameObject>();
    public GameObject CurrentBuilding;
    private Util.MyRect _bound;
    private MeshRenderer _currentMesh;
    private Material _originalMaterial;
    private Material _redMaterial;
    private WorkerController _worker;
    public Action CancelBuild;
    public void Init()
    {
        _originalMaterial = Resources.Load<Material>("Material/BuildingOriginal");
        _redMaterial = Resources.Load<Material>("Material/BuildingRed");
        Managers.Input.OnMouseEvent += Create;
    }
    
    public void BuildShadow(Define.BuildList building)
    {
        var builders = Managers.Instance.UnitController.SelectUnit.FindAll(unit => unit is IBuilder);

        foreach (var item in builders)
        {
            if (item.CurrentState is Unit.State.BuildState)
                continue;
            
            CurrentBuilding = Managers.Resources.Activation($"Building/{building.ToString()}", null);
            _currentMesh = CurrentBuilding.GetComponent<MeshRenderer>();
            _worker = item as WorkerController;
            break;
        }
    }

    private void Create(Define.MouseEventType type)
    {
        if (!CurrentBuilding)
            return;

        switch (type)
        {
            case Define.MouseEventType.RightClick:
                CanselToCreate();
                break;
            case Define.MouseEventType.LeftClick:
                if (_canBuild)
                    SuccessToCreate(50);
                break;
        }
    }

    private void SuccessToCreate(int gold)
    {
        Managers.Instance.Node.SetNode(_bound);
        Managers.Game.SetGoldEvent(50);
        CurrentBuilding.GetComponent<BuildingBase>().Init();
        _building.Add(CurrentBuilding);
        _worker.SetBuildState(CurrentBuilding);
        CurrentBuilding = null;
    }
    private void CanselToCreate()
    {
        CancelBuild?.Invoke();
        Managers.Resources.Release(CurrentBuilding);
        CurrentBuilding = null;
    }
    public void OnUpdate()
    {
        if (!CurrentBuilding)
            return;

        SetBoundSize();

        var units = Managers.Game.GetAllUnitList();
        var node = Managers.Instance.Node.Nodes;
        
        foreach (var t in units)
        {
            if (!_bound.Contains(t.transform.position.x, t.transform.position.z)) continue;
            _currentMesh.material = _redMaterial;
            return;
        }
        
        foreach (var item in node)
        {
            if (!_bound.Contains(item.PosX, item.PosZ)) continue;
            switch (item.NodeTypes)
            {
                case NodeTypes.Building:
                    _canBuild = false;
                    _currentMesh.material = _redMaterial;
                    return;
                case NodeTypes.None:
                    _canBuild = true;
                    _currentMesh.material = _originalMaterial;
                    break;
            }
        }
        
    }

    private void SetBoundSize()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.WorldToScreenPoint(CurrentBuilding.transform.position).z;
        Vector3 objectPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        CurrentBuilding.transform.position = new Vector3(objectPosition.x,2f,objectPosition.z);
        _bound.MinX = CurrentBuilding.transform.position.x - _currentMesh.bounds.size.x / 2;
        _bound.MaxX = CurrentBuilding.transform.position.x + _currentMesh.bounds.size.x / 2;
        _bound.MinZ = CurrentBuilding.transform.position.z - _currentMesh.bounds.size.z / 2;
        _bound.MaxZ = CurrentBuilding.transform.position.z + _currentMesh.bounds.size.z / 2;
    }
}
