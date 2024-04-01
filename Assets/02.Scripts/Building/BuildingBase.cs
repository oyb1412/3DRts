using System;
using System.Collections.Generic;
using Building.State;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UI;

public abstract class BuildingBase : MonoBehaviour, IHit, IUIBehavior, IAllUnit
{
    public enum BuildingType
    {
        Castle,
        Barrack,
        Tower,
    }

    public BuildingType MyType;
    public IBuildingState BuildState;
    protected IUIBehavior UIBehavior;
    private BuildState _state;
    public WorkerController Worker;
    public GameObject _selectMarker;
    public GameObject _hpBar;
    private Define.Select _select;
    private NavMeshObstacle _nmo;
    [HideInInspector]public BuildingStatus MyStatus;
    public abstract void SetSelectedState(Define.Select selectedState);

    protected void DefaultSelectedState(Define.Select selectedState)
    {
        switch (selectedState)
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

        _select = selectedState;
    }
    public Action OnBuildCompleteEvent;
    public Action<float> OnHpEvent { get; set; }
    public Action<float> OnBuildEvent { get; set; }
    public Action DeleteHpBarEvent { get; set; }
    private void Awake()
    {
        BuildState = new IdleState();
        _hpBar = Util.FindChild(gameObject, "HpBar");
        _selectMarker = Util.FindChild(gameObject, "SelectMarker");
        _hpBar.SetActive(false);
        _selectMarker.SetActive(false);
        _nmo = GetComponent<NavMeshObstacle>();
        MyStatus = Util.GetOrAddComponent<BuildingStatus>(gameObject);
    }

    public abstract void Init();
    protected virtual void Start()
    {
        MyStatus.MaxHp = 100;
        MyStatus.Hp = MyStatus.MaxHp;
        MyStatus.CurrentBuildingTime = 0f;
        MyStatus.MaxBuildingTime = 10f;
        Init();
    }

    public void Hit(IAttackerStatus status)
    {
        int attack = Mathf.Max(status.AttackDamage - MyStatus.Defense, 1);
        MyStatus.Hp -= attack;
        OnHpEvent?.Invoke(MyStatus.Hp / MyStatus.MaxHp);
        if (MyStatus.Hp <= 0)
        {
            BuildState = new DestroyState();
            
            //todo
            //파괴된 지역 재사용 가능하게
        }
    }

    public void SetBuilding(GameObject go)
    {
        _nmo.enabled = true;
        BuildState = new BuildState();
        Worker = go.GetComponent<WorkerController>();
    }

    private void Update()
    {
        BuildState.OnUpdate(this);
    }

    public void UpdateUI(List<GameObject> panels)
    {
        UIBehavior?.UpdateUI(panels);
        if (BuildState is BuildState)
            Managers.Instance.UIBehaviourPanel.HideUI();
    }

}
