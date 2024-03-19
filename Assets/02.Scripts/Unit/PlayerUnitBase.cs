using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UI;

public abstract class PlayerUnitBase : MonoBehaviour, IHit, IUIBehavior, IAllUnit
{
    public enum Type
    {
        None,
        SwordMan,
        Worker,
        Archer,
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
    [HideInInspector]public Vector3 DestPos;
    [HideInInspector]public GameObject LockTarget;
    [HideInInspector]public Type MyType;

    private Define.Select _select;
    [HideInInspector]public UnitStatus Status;
    
    [HideInInspector]public NavMeshAgent Nma;
    public Animator Anime;
    public IUnitState CurrentState;
    private GameObject _selectMarker;
    private GameObject _hpBar;
    [HideInInspector]public float ScanRange = 5f;

    public Action<float> OnHpEvent { get; set; }
    public Action DeleteHpBarEvent { get; set; }

    private void Awake()
    {
        _hpBar = Util.FindChild(gameObject, "HpBar");
        _selectMarker = Util.FindChild(gameObject, "SelectMarker");
        _hpBar.SetActive(false);
        _selectMarker.SetActive(false);
    }

    protected virtual void Start()
    {
        Status = Util.GetOrAddComponent<UnitStatus>(gameObject);
        Nma = GetComponent<NavMeshAgent>();
        Anime = GetComponent<Animator>();
        CurrentState = new Unit.State.IdleState(this);
        Status.MoveSpeed = 4;
        Status.Hp = 50;
        Status.AttackDamage = 5;
        Status.AttackRange = 2;
        Status.MaxHp = 50;
        Nma.speed = Status.MoveSpeed;

    }
    void Update()
    {
        CurrentState.OnUpdate(this);
    }
    protected abstract void OnAttackEvent();
    public void OnDieEvent()
    {
        DeleteHpBarEvent?.Invoke();
        Managers.Pool.Release(GetComponent<Poolable>());
    }

    public void SetState(IUnitState newState)
    {
        CurrentState = newState;
    }
    
    public void Hit(IAttackerStatus status)
    {
        int attack = Mathf.Max(status.AttackDamage - Status.Defense, 1);
        Status.Hp -= attack;
        OnHpEvent?.Invoke(Status.Hp / Status.MaxHp);
        if (Status.Hp <= 0)
        {
            SetState(new Unit.State.DieState(this));
        }
    }

    public void PlayerUnitMove(Vector3 destPos, IUnitState state)
    {
        if (CurrentState is Unit.State.BuildState)
            return;
        
        DestPos = destPos;
        LockTarget = null;
        SetState(state);
    }

    public void PlayerUnitAttack(GameObject target, IUnitState state)
    {
        if (CurrentState is Unit.State.BuildState)
            return;
        
        LockTarget = target;
        SetState(state);
    }

    public void UpdateUI(Dictionary<string, GameObject> panels)
    {
        UIBehavior?.UpdateUI(panels);

    }

    public GameObject GetThisObject()
    {
        return gameObject;
    }

    public GameObject GetThisObjectType()
    {
        return gameObject;
    }
}
