using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherController : PlayerUnitBase, IState, IAttacker
{
    [SerializeField] private Transform _firePos;
    protected override void Start()
    {
        base.Start();
        _myType = Type.Archer;
        _myStateMachine = new StateMachine();
        _myStateMachine.Init(this, this);
    }

    private void UpdateIdle()
    {
        
    }
    private void UpdateMove()
    {
 
        if (_lockTarget)
        {
            _destPos.y = transform.position.y;
            float targetDir = (_lockTarget.transform.position - transform.position).magnitude;
            if (targetDir <= _status.AttackRange)
            {
                MyState = Define.State.Attack;
            }
            else
            {
                _nma.SetDestination(_lockTarget.transform.position);
            }
        }
        else
        {
            _destPos.y = transform.position.y;
            float dir = (_destPos - transform.position).magnitude;
            if (dir <= 0.1f)
            {
                MyState = Define.State.Idle;
            }
            else
            {
                _nma.SetDestination(_destPos);
            }
        }
    }
    
    private void UpdateAttack()
    {
        if (_lockTarget.GetComponent<EnemyUnitBase>().MyState == Define.State.Die)
        {
            MyState = Define.State.Idle;
            return; 
        }
        transform.LookAt(_lockTarget.transform.position);
    }
    
    private void UpdateDie()
    {
        
    }

    private void UpdatePatrol()
    {
        _destPos.y = transform.position.y;
        float dir = (_destPos - transform.position).magnitude;
        if (dir <= 0.1f)
        {
            MyState = Define.State.Idle;
        }
        else
        {
            _nma.SetDestination(_destPos);
            int mask = (1 << (int)Define.Layer.Monster);
            var monsters = Physics.OverlapSphere(transform.position, scanRange, mask);
            var monster = Util.SortToShotDistance(monsters, transform);
            if (monster != null)
            {
                _lockTarget = monster;
                MyState = Define.State.Move;
            }
        }
    }
    
    protected override void OnAttackEvent()
    {
        if (_lockTarget == null)
            return;

        FireProjectile();
    }

    private void FireProjectile()
    {
        Vector3 dir = (_lockTarget.transform.position - transform.position).normalized;
        ProjectileController arrow = Managers.Resources.Activation("ArrowParent", null).GetComponent<ProjectileController>();
        arrow.transform.position = _firePos.position;
        arrow.Init(_lockTarget, dir, transform.position, _status);
    }

    public void OnUpdateState()
    {
        switch (MyState)
        {
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Attack:
                UpdateAttack();
                break;
            case Define.State.Die:
                UpdateDie();
                break;
            case Define.State.Move:
                UpdateMove();
                break;
            case Define.State.Patrol:
                UpdatePatrol();
                break;
        }
    }

    public void OnChangeState(Define.State state)
    {
        MyState = state;
    }
}
