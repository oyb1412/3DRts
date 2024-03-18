using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatus : BaseStatus, IAttackerStatus
{
    public float MoveSpeed { get; set; }
    
    public float AttackSpeed { get; set; }
    public float AttackRange { get; set; }
    public int AttackDamage { get; set; }
}
