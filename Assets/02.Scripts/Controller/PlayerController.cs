using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum State
    {
        None,
        Idle,
        Move,
        Attack,
        Die,
        Reload,
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
                    _anime.CrossFade("Move", .2f);
                    break;
                case State.Idle:
                    _anime.CrossFade("Idle", .2f);
                    break;
                case State.Attack:
                    _anime.CrossFade("Attack", .2f);
                    break;
            }
            _state = value;
        }
    }
    
    [SerializeField]private Vector3 _destPos;
    [SerializeField]private State _state;
    [SerializeField]private float _speed = 10f;
    [SerializeField]private Animator _anime;
    [SerializeField]private GameObject _lockTarget;
    



    void Start()
    {
        Managers.Input.OnMouseEvent += OnMouseEvent;
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
        }
    }
    private void UpdateIdle()
    {
        
    }
    private void UpdateMove()
    {
        float dir = (_destPos - transform.position).magnitude;
        if (dir < 0.1f)
        {
            MyState = State.Idle;
            return;
        }
        else
        {
            Vector3 dist = (_destPos - transform.position).normalized;
            transform.position += dist * (_speed * Time.deltaTime);
            transform.LookAt(_destPos);
        }
    }
    
    
    private void UpdateAttack()
    {
        transform.LookAt(_lockTarget.transform.position);
    }
    
    private void OnMouseEvent(Define.MouseEventType type)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position,ray.direction * 100f,Color.red,1f);
        RaycastHit hit;
        int mask =  (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
        bool raycastHit = Physics.Raycast(ray, out hit, float.MaxValue, mask);

        switch (type)
        {
            case Define.MouseEventType.RightClick:
            {
                if (raycastHit)
                {
                    if(hit.collider.gameObject.layer == (int)Define.Layer.Ground)
                    {
                        _destPos = hit.point;
                        MyState = State.Move;
                    }
                    else if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                    {
                        MyState = State.Attack;
                        _lockTarget = hit.collider.gameObject;
                    }
                }
            }
                break;
        }
    }
}
