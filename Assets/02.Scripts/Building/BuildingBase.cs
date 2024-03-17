using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class BuildingBase : MonoBehaviour, IHit, IUIBehavior, IInstallation
{
    public enum BuildingType
    {
        Castle,
        Barrack,
        Tower,
    }
    public Define.Select MySelect
    {
        get
        {
            return _select;
        }
        set
        {
            switch (value)
            {
                case Define.Select.Deselect:
                    _hpBar.SetActive(false);
                    _selectMarker.SetActive(false);
                    break;
                case Define.Select.Select:
                    _hpBar.SetActive(true);
                    _selectMarker.SetActive(true);
                    break;
            }
            _select = value;
        }
    }

    public BuildingType MyType;
    public IBuildingState BuildState;
    protected IUIBehavior UIBehavior;
    private BuildState _state;
    public WorkerController _myWorker;
    private GameObject _selectMarker;
    private GameObject _hpBar;
    private Define.Select _select;
    private NavMeshObstacle _nmo;
    [HideInInspector]public BaseStatus MyStatus;
    private void Awake()
    {
        BuildState = new BuildingIdleState();
        _hpBar = Util.FindChild(gameObject, "HpBar");
        _selectMarker = Util.FindChild(gameObject, "SelectMarker");
        _hpBar.SetActive(false);
        _selectMarker.SetActive(false);
        _nmo = GetComponent<NavMeshObstacle>();
        MyStatus = GetComponent<BuildingStatus>();
        MyStatus.Hp = 0;
        MyStatus.MaxHp = 100;
    }

    protected virtual void Start()
    {
        
    }

    public void Hit(BaseStatus status)
    {
        int attack = Mathf.Max(status.AttackDamage - MyStatus.Defense, 1);
        MyStatus.Hp -= attack;
        OnHpEvent?.Invoke(MyStatus.Hp / MyStatus.MaxHp);
        if (MyStatus.Hp <= 0)
        {
            BuildState = new BuildingDestroyState();
            
            //todo
            //파괴된 지역 재사용 가능하게
        }
    }

    public void SetBuilding(GameObject go)
    {
        _nmo.enabled = true;
        BuildState = new BuildingBuildState();
        _myWorker = go.GetComponent<WorkerController>();
    }

    private void Update()
    {
        BuildState.OnUpdate(this);
    }

    public Action<float> OnHpEvent { get; set; }
    public Action DeleteHpBarEvent { get; set; }

    public void UpdateUI(Dictionary<string, GameObject> panels)
    {
        UIBehavior?.UpdateUI(panels);
        if (BuildState is BuildingBuildState)
            Managers.Instance.UIBehaviourPanel.HideUI();
    }

    public GameObject GetThisObject()
    {
        return gameObject;
    }
}
