using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class PlayerUnitBase : MonoBehaviour, IHit, IUIBehavior
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
    private Animator _anime;
    public IUnitState CurrentState = new IdleState();
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
        _anime = GetComponent<Animator>();
        Status.MoveSpeed = 10;
        Status.Hp = 50;
        Status.AttackPower = 5;
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
        switch (newState)
        {
            case MoveState:
            case PatrolState:
            case BuildMoveState:
            {
                _anime.CrossFade("Move", .2f);
            }
                break;
            case IdleState:
            case HoldState:
            {
                Nma.SetDestination(transform.position);
                _anime.CrossFade("Idle", .2f);
            }
                break;
            case AttackState:
            {
                Nma.SetDestination(transform.position);
                _anime.CrossFade("Attack", .2f);
            }
                break;
            case DieState:
            {
                GetComponent<Collider>().enabled = false;
                Nma.SetDestination(transform.position);
                _anime.CrossFade("Die", .2f);
            }
                break;
            case BuildState:
            {
                Nma.SetDestination(transform.position);
                _anime.CrossFade("Build", .2f);
            }
                break;
        }
        CurrentState = newState;
    }
    
    
    public void Hit(UnitStatus status)
    {
        int attack = Mathf.Max(status.AttackPower - Status.Defense, 1);
        Status.Hp -= attack;
        OnHpEvent?.Invoke(Status.Hp / Status.MaxHp);
        if (Status.Hp <= 0)
        {
            SetState(new DieState());
        }
    }

    public void PlayerUnitMove(Vector3 destPos, IUnitState state)
    {
        if (CurrentState is BuildState)
            return;
        
        DestPos = destPos;
        LockTarget = null;
        SetState(state);
    }

    public void PlayerUnitAttack(GameObject target, IUnitState state)
    {
        if (CurrentState is BuildState)
            return;
        
        LockTarget = target;
        SetState(state);
    }

    public void UpdateUI(Dictionary<string, GameObject> panels)
    {
        UIBehavior?.UpdateUI(panels);

    }
}
