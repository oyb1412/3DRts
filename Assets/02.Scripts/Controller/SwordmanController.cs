using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordmanController : PlayerUnitBase, IState, IAttacker
{

    protected override void Start()
    {
        base.Start();
        _myType = Type.Swordman;
        _myStateMachine = new StateMachine();
        _myStateMachine.Init(this, this);
    }

    protected void UpdateIdle()
    {
        
    }
    protected void UpdateMove()
    {
 
        if (_lockTarget)
        {
            _destPos.y = transform.position.y;
            float targetDir = (_lockTarget.transform.position - transform.position).magnitude;
            if (targetDir <= _status.AttackRange)
            {
                _myStateMachine.OnChange(Define.State.Attack);
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
                _myStateMachine.OnChange(Define.State.Idle);
            }
            else
            {
                _nma.SetDestination(DestPos);
            }
        }
    }
    protected void UpdateAttack()
    {
        if (_lockTarget.GetComponent<EnemyUnitBase>().MyState == Define.State.Die)
        {
            _myStateMachine.OnChange(Define.State.Idle);
            return; 
        }
        transform.LookAt(_lockTarget.transform.position);
    }
    protected void UpdateDie()
    {
        
    }
    protected void UpdatePatrol()
    {
       _destPos.y = transform.position.y;
       float dir = (_destPos - transform.position).magnitude;
       if (dir <= 0.1f)
       {
           _myStateMachine.OnChange(Define.State.Idle);
       }
       else
       {
           _nma.SetDestination(_destPos);
           int mask = (1 << (int)Define.Layer.Monster);
           var monsters = Physics.OverlapSphere(transform.position, scanRange, mask);
           var monster = Util.SortToShotDistance(monsters, transform);
           if (monster != null)
           {
               _myStateMachine.OnChange(Define.State.Move);
               _lockTarget = monster;
           }
       }
    }

    private void UpdateHold()
    {
        int mask = (1 << (int)Define.Layer.Monster);
        var monsters = Physics.OverlapSphere(transform.position, scanRange, mask);
        var monster = Util.SortToShotDistance(monsters, transform);
        if (monster != null)
        {
            _lockTarget = monster;
            MyState = Define.State.Move;
        }
    }
    
    protected override void OnAttackEvent()
    {
        if (_lockTarget == null)
            return;
        
        _lockTarget.GetComponent<IHit>().Hit(_status);
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
            case Define.State.Hold:
                UpdateHold();
                break;
        }
    }

    public void OnChangeState(Define.State state)
    {
        MyState = state;
    }
}
