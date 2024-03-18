using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UI;

public abstract class BuildingBase : MonoBehaviour, IHit, IUIBehavior, IInstallation, IAllUnit
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
                    OnBuildEvent = null;
                    OnHpEvent = null;
                    OnCreateSliderEvent = null;
                    OnCreateImageEvent = null;
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
    public WorkerController Worker;
    private GameObject _selectMarker;
    private GameObject _hpBar;
    private Define.Select _select;
    private NavMeshObstacle _nmo;
    private float SiteRange = 10f;
    private float SiteTime = 1f;
    [HideInInspector]public BuildingStatus MyStatus;
    
    public Action<float> OnHpEvent { get; set; }
    public Action<float> OnBuildEvent { get; set; }
    public Action<float> OnCreateSliderEvent { get; set; }
    public Action<int> OnCreateImageEvent { get; set; }
    public Action OnCreateCompleteEvent { get; set; }
    public Action OnCreateStartEvent { get; set; }
    public Action DeleteHpBarEvent { get; set; }
    private void Awake()
    {
        BuildState = new BuildingIdleState();
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
    }

    public void Hit(IAttackerStatus status)
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
        Worker = go.GetComponent<WorkerController>();
    }

    private void Update()
    {
        BuildState.OnUpdate(this);
        
        SiteTime += Time.deltaTime;

        if (SiteTime < 1)
            return;
        
        var node = Managers.Instance.Node;
        node.UpdateNodesColor(transform, SiteRange);

        SiteTime = 0;
    }



    public void UpdateUI(Dictionary<string, GameObject> panels)
    {
        UIBehavior?.UpdateUI(panels);
        if (BuildState is BuildingBuildState)
            Managers.Instance.UIBehaviourPanel.HideUI();
    }

 

    public GameObject GetThisObjectType()
    {
        return gameObject;
    }

    public GameObject GetThisObject()
    {
        return gameObject;
    }
}
