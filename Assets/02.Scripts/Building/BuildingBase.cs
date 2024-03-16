using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingBase : MonoBehaviour, IHit, IUIBehavior
{
    public enum CreatingType
    {
        None,
        Creating,
        Complete,
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
    
    protected IUIBehavior UIBehavior;
    public CreatingType _myCreatingType = CreatingType.None;
    private WorkerController _myWorker;
    private GameObject _selectMarker;
    private GameObject _hpBar;
    private Define.Select _select;
    public BuildingStatus MyStatus;
    private void Awake()
    {
        _hpBar = Util.FindChild(gameObject, "HpBar");
        _selectMarker = Util.FindChild(gameObject, "SelectMarker");
        _hpBar.SetActive(false);
        _selectMarker.SetActive(false);
        MyStatus = GetComponent<BuildingStatus>();
        MyStatus.Hp = 0;
        MyStatus.MaxHp = 100;
    }

    protected virtual void Start()
    {
        
    }

    public void Hit(UnitStatus status)
    {
        
    }

    public void SetCreate(GameObject go)
    {
        _myCreatingType = CreatingType.Creating;
        _myWorker = go.GetComponent<WorkerController>();
    }
    
    private void Update()
    {
        if (_myCreatingType != CreatingType.Creating)
            return;

        
        MyStatus.Hp += 5 * Time.deltaTime;
        OnHpEvent?.Invoke(MyStatus.Hp);

        if (MyStatus.Hp >= MyStatus.MaxHp)
        {
            _myCreatingType = CreatingType.Complete;
            _myWorker.SetState(new IdleState());
        }
    }

    public Action<float> OnHpEvent { get; set; }
    public Action DeleteHpBarEvent { get; set; }

    public void UpdateUI(Dictionary<string, GameObject> panels)
    {
        UIBehavior?.UpdateUI(panels);
    }
}
