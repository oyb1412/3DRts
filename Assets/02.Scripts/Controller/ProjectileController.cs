using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private Rigidbody _rigid;
    private Status _status;
    private GameObject _target;
    [SerializeField] private float _speed;

    public void Init(GameObject target, Vector3 dir, Vector3 pos, Status status)
    {
        transform.position = pos;
        _rigid = GetComponent<Rigidbody>();
        _rigid.velocity = dir * _speed;
        transform.LookAt(target.transform.position);
        _target = target;
        _status = status;
    }

    private void Update()
    {
         if (Vector3.Distance(transform.position, _target.transform.position) < 0.1f)
         {
             _target.GetComponent<IHit>().IHit(_status);
             Managers.Resources.Release(gameObject);
         }
    }
}
