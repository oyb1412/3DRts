using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private Status _status;
    private GameObject _target;
    private Vector3 _dir;
    [SerializeField] private float _speed;

    public void Init(GameObject target, Vector3 dir, Vector3 pos, Status status)
    {
        _target = target;
        _status = status;
    }

    private void Update()
    {
         if (Vector3.Distance(transform.position, _target.transform.position) < 0.1f)
         {
             _target.GetComponent<IHit>().Hit(_status);
             Managers.Resources.Release(gameObject);
         }
         else
         {
             transform.LookAt(_target.transform.position);
             _dir = (_target.transform.position - transform.position).normalized;
             transform.position += _dir * (Time.deltaTime * _speed);
         }
    }
}
