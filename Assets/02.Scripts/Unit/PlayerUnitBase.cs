using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class PlayerUnitBase : MonoBehaviour, IHit
{
    public enum Type
    {
        None,
        Swordman,
        Worker,
        Archer,
    }
    public enum Select
    {
        None,
        Select,
        Deselect,
    }
    public enum State
    {
        None,
        Idle,
        Move,
        Attack,
        Die,
        Patrol,
    }
    
    public Select MySelect
    {
        get
        {
            return _select;
        }
        set
        {
            switch (value)
            {
                case Select.Deselect:
                    _hpBar.SetActive(false);
                    _selectedMarker.SetActive(false);
                    break;
                case Select.Select:
                    _hpBar.SetActive(true);
                    _selectedMarker.SetActive(true);
                    break;
            }
            _select = value;
        }
    }
    public State MyState
    {
        get
        {
            return _state;
        }
        set
        {
            switch (value)
            {
                case State.Move:
                {
                    _anime.CrossFade("Move", .2f);
                }
                    break;
                case State.Idle:
                {
                    _nma.SetDestination(transform.position);
                    _anime.CrossFade("Idle", .2f);
                }
                    break;
                case State.Attack:
                {
                    _nma.SetDestination(transform.position);
                    _anime.CrossFade("Attack", .2f);
                }
                    break;
                case State.Die:
                {
                    GetComponent<Collider>().enabled = false;
                    _nma.SetDestination(transform.position);
                    _anime.CrossFade("Die", .2f);
                }
                    break;
                case State.Patrol:
                {
                    _anime.CrossFade("Move", .2f);
                }
                    break;
            }
            _state = value;
        }
    }
    public Type MyType;

    public Vector3 DestPos
    {
        get { return _destPos; }
        set { _destPos = value; }
    }
    protected Vector3 _destPos;
    protected Select _select;
    protected State _state = State.Idle;
    protected Status _status;
    protected NavMeshAgent _nma;
    protected Animator _anime;
    protected GameObject _lockTarget;
    [SerializeField]protected GameObject _selectedMarker;
    [SerializeField]protected GameObject _hpBar;
    [SerializeField]protected float scanRange;

    public Action<float> OnHpEvent { get; set; }
    public GameObject LockTarget
    {
        get { return _lockTarget; }
        set { _lockTarget = value; }
    }

    protected virtual void Start()
    {
        _nma = GetComponent<NavMeshAgent>();
        _anime = GetComponent<Animator>();
        _status = Util.GetorAddComponent<Status>(gameObject);
        _nma.speed = _status.MoveSpeed;
        MyState = State.Idle;
        _hpBar.SetActive(false);
    }
    
    void Update()
    {
        switch (_state)
        {
            case State.Idle:
                UpdateIdle();
                break;
            case State.Move:
                UpdateMove();
                break;
            case State.Attack:
                UpdateAttack();
                break;
            case State.Die:
                UpdateDie();
                break;
            case State.Patrol:
                UpdatePatrol();
                break;
        }
    }

    protected abstract void UpdateIdle();
    protected abstract void UpdateAttack();
    protected abstract void UpdateMove();
    protected abstract void UpdateDie();
    protected abstract void UpdatePatrol();
    protected abstract void OnAttackEvent();


    public void OnDieEvent()
    {
        Destroy(_hpBar.gameObject);
        Managers.Pool.Release(GetComponent<Poolable>());
    }
     
    public void IHit(Status status)
    {
        int attack = Mathf.Max(status.AttackPower - _status.Defense, 1);
        _status.Hp -= attack;
        OnHpEvent?.Invoke(_status.Hp / _status.MaxHp);
        if (_status.Hp <= 0)
        {
            MyState = State.Die;
        }
    }

    public void PlayerUnitMove(Vector3 destPos, State state)
    {
        DestPos = destPos;
        LockTarget = null;
        MyState = state;
    }

    public void PlayerUnitAttack(State state, GameObject target)
    {
        MyState = state;
        LockTarget = target;
    }
}
