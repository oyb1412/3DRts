using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : EnemyUnitBase, IState
{
    protected override void Start()
    {
        base.Start();
        _myType = Type.Zombie;
        _myStateMachine = new StateMachine();
        _myStateMachine.Init(this, this);
    }

    private void UpdateIdle()
    {
        int mask = (1 << (int)Define.Layer.Player);
        var players = Physics.OverlapSphere(transform.position, scanRange, mask);
        var player = Util.SortToShotDistance(players, transform);
        if (player != null)
        {
            _lockTarget = player;
            MyState = Define.State.Move;
        }
    }

    private void UpdateAttack()
    {
        if (_lockTarget.GetComponent<PlayerUnitBase>().MyState == Define.State.Die)
        {
            MyState = Define.State.Idle;
            return;
        }
        
        float targetDir = (_lockTarget.transform.position - transform.position).magnitude;
        if (targetDir > _status.AttackRange)
        {
            MyState = Define.State.Move;
            return;
        }
        transform.LookAt(_lockTarget.transform.position);
    }

    private void UpdateMove()
    {
        if (_lockTarget)
        {
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
            MyState = Define.State.Idle;
        }
    }

    private void UpdateDie()
    {
        
    }

    private void UpdatePatrol()
    {
        
    }

    public void OnUpdateState()
    {
        switch (_state)
        {
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Move:
                UpdateMove();
                break;
            case Define.State.Attack:
                UpdateAttack();
                break;
            case Define.State.Die:
                UpdateDie();
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
