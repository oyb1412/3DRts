using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : BaseController
{
    public enum Type
    {
        None,
        Marin,
    }
    

    public Type MyType = Type.Marin;
    [SerializeField]private Vector3 _destPos;
    public Vector3 DestPos
    {
        get { return _destPos; }
        set { _destPos = value; }
    }

    protected override void UpdateIdle()
    {
        
    }
    protected override void UpdateMove()
    {
 
        if (_lockTarget)
        {
            _destPos.y = transform.position.y;
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
            _destPos.y = transform.position.y;
            float dir = (_destPos - transform.position).magnitude;
            if (dir <= 0.1f)
            {
                MyState = State.Idle;
            }
            else
            {
                _nma.SetDestination(_destPos);
            }
        }
    }
    
    protected override void UpdateAttack()
    {
        if (_lockTarget.GetComponent<BaseController>().MyState == State.Die)
        {
            MyState = State.Idle;
            return; 
        }
        transform.LookAt(_lockTarget.transform.position);

    }
    
    protected override void UpdateDie()
    {
        
    }

    protected override void UpdatePatrol()
    {
       _destPos.y = transform.position.y;
       float dir = (_destPos - transform.position).magnitude;
       if (dir <= 0.1f)
       {
           MyState = State.Idle;
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
               MyState = State.Move;
           }
       }
    }
}
