using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : EnemyUnitBase
{
    protected override void UpdateIdle()
    {
        int mask = (1 << (int)Define.Layer.Player);
        var players = Physics.OverlapSphere(transform.position, scanRange, mask);
        var player = Util.SortToShotDistance(players, transform);
        if (player != null)
        {
            _lockTarget = player;
            MyState = State.Move;
        }
    }

    protected override void UpdateAttack()
    {
        if (_lockTarget.GetComponent<PlayerUnitBase>().MyState == PlayerUnitBase.State.Die)
        {
            MyState = State.Idle;
            return;
        }
        
        float targetDir = (_lockTarget.transform.position - transform.position).magnitude;
        if (targetDir > _status.AttackRange)
        {
            MyState = State.Move;
        }
    }

    protected override void UpdateMove()
    {
        if (_lockTarget)
        {
            float targetDir = (_lockTarget.transform.position - transform.position).magnitude;
            if (targetDir <= _status.AttackRange)
            {
                MyState = State.Attack;
            }
            else
            {
                _nma.SetDestination(_lockTarget.transform.position);
            }
        }
        else
        {
            MyState = State.Idle;
        }
    }

    protected override void UpdateDie()
    {
        
    }

    protected override void UpdatePatrol()
    {
        
    }
}
