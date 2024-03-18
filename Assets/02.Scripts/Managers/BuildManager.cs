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
        //선택중인 유닛 중에 빌더가 있는지
        var builders = Managers.Instance.UnitController.SelectUnit.FindAll(unit => unit is IBuilder);

        foreach (var item in builders)
        {
            //빌더중에 buildstate가 아닌 빌더가 있는지
            if (item.CurrentState is not BuildState)
            {
                //todo
                //비용이 충분할 때만 설치 가능하도록
                //if (build._myStatus.CreateCost <= Managers.Game.CurrentGold)
                {
                    CurrentBuilding = Managers.Resources.Activation($"Building/{building.ToString()}", null);
                    _currentMesh = CurrentBuilding.GetComponent<MeshRenderer>();
                    _worker = item as WorkerController;
                    break;
                    //Managers.Game.CurrentGold -= build._myStatus.CreateCost;
                }
            }
        }
    }

    private void Create(Define.MouseEventType type)
    {
        if (!CurrentBuilding)
            return;

        switch (type)
        {
            case Define.MouseEventType.RightClick:
                CancelBuild?.Invoke();
                Managers.Resources.Release(CurrentBuilding);
                CurrentBuilding = null;
                break;
            case Define.MouseEventType.LeftClick:
                if (_canBuild)
                {
                    Managers.Instance.Node.SetNode(_bound);
                    CurrentBuilding.GetComponent<BuildingBase>().Init();
                    _building.Add(CurrentBuilding);
                    _worker.SetBuildState(CurrentBuilding);
                    Managers.Game.SetGoldEvent(50);
                    CurrentBuilding = null;
                }
                break;
        }
    }
    public void OnUpdate()
    {
        if (!CurrentBuilding)
            return;
        
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.WorldToScreenPoint(CurrentBuilding.transform.position).z;
        Vector3 objectPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        CurrentBuilding.transform.position = new Vector3(objectPosition.x,2f,objectPosition.z);
        _bound.MinX = CurrentBuilding.transform.position.x - _currentMesh.bounds.size.x / 2;
        _bound.MaxX = CurrentBuilding.transform.position.x + _currentMesh.bounds.size.x / 2;
        _bound.MinZ = CurrentBuilding.transform.position.z - _currentMesh.bounds.size.z / 2;
        _bound.MaxZ = CurrentBuilding.transform.position.z + _currentMesh.bounds.size.z / 2;

        var units = GameObject.FindGameObjectsWithTag("Obstacle");
        var node = Managers.Instance.Node.Buildings;
        
        foreach (var item in units)
        {
            if (!_bound.Contains(item.transform.position.x, item.transform.position.z)) continue;
            _currentMesh.material = _redMaterial;
            return;
        }
        
        foreach (var item in node)
        {
            if (!_bound.Contains(item.X, item.Z)) continue;
            switch (item.BnteractableTypes)
            {
                case InteractableTypes.Building:
                    _canBuild = false;
                    _currentMesh.material = _redMaterial;
                    return;
                case InteractableTypes.None:
                    _canBuild = true;
                    _currentMesh.material = _originalMaterial;
                    break;
            }
        }
        
    }

}
