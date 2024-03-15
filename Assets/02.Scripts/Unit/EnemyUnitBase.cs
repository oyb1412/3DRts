using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyUnitBase : MonoBehaviour, IHit
{
    public enum Type
    {
        None,
        Zombie
    }
    public enum Select
    {
        None,
        Select,
        Deselect,
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
    public Define.State MyState
    {
        get
        {
            return _state;
        }
        set
        {
            switch (value)
            {
                case Define.State.Move:
                {
                    _anime.CrossFade("Move", .2f);
                }
                    break;
                case Define.State.Idle:
                {
                    _nma.SetDestination(transform.position);
                    _anime.CrossFade("Idle", .2f);
                }
                    break;
                case Define.State.Attack:
                {
                    _nma.SetDestination(transform.position);
                    _anime.CrossFade("Attack", .2f);
                }
                    break;
                case Define.State.Die:
                {
                    GetComponent<Collider>().enabled = false;
                    _nma.SetDestination(transform.position);
                    _anime.CrossFade("Die", .2f);
                }
                    break;
                case Define.State.Patrol:
                {
                    _anime.CrossFade("Move", .2f);
                }
                    break;
            }
            _state = value;
        }
    }
    

    public Vector3 DestPos
    {
        get { return _destPos; }
        set { _destPos = value; }
    }
    [SerializeField]protected Vector3 _destPos;
    protected Select _select;
    protected Type _myType;
    [SerializeField]protected Define.State _state = Define.State.Idle;
    protected Status _status;
    protected NavMeshAgent _nma;
    protected Animator _anime;
    protected StateMachine _myStateMachine;

    [SerializeField]protected float scanRange;
    [SerializeField]protected GameObject _lockTarget;
    [SerializeField]protected GameObject _selectedMarker;
    [SerializeField]protected GameObject _hpBar;
    public Action<float> OnHpEvent { get; set; }
    public Action DeleteHpBarEvent { get; set; }

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
        MyState = Define.State.Idle;
        _hpBar.SetActive(false);
    }
    
    void Update()
    {
        _myStateMachine.OnUpdate();

    }

    public void OnAttackEvent()
    {
        if (_lockTarget == null)
            return;
        
        _lockTarget.GetComponent<IHit>().Hit(_status);
    }

    public void OnDieEvent()
    {
        DeleteHpBarEvent?.Invoke();
        Managers.Pool.Release(GetComponent<Poolable>());
    }
    
    public void Hit(Status status)
    {
        int attack = Mathf.Max(status.AttackPower - _status.Defense, 1);
        _status.Hp -= attack;
        OnHpEvent?.Invoke(_status.Hp / _status.MaxHp);
        if (_status.Hp <= 0)
        {
            MyState = Define.State.Die;
        }
    }
}
