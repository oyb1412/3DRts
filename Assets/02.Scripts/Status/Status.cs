using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField]private float _moveSpeed;
    [SerializeField]private float _hp;
    [SerializeField]private float _maxHp;
    [SerializeField]private int _attackPower;
    [SerializeField]private float _attackRange;
    [SerializeField]private int _defense;

    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
    public float Hp { get { return _hp; } set { _hp = value; } }
    public float MaxHp { get { return _maxHp; } set { _maxHp = value; } }
    public int AttackPower { get { return _attackPower; } set { _attackPower = value; } }
    public int Defense { get { return _defense; } set { _defense = value; } }
    public float AttackRange { get { return _attackRange; } set { _attackRange = value; } }

    private void Start()
    {
       
    }
}
