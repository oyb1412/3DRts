using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatus : BaseStatus
{
    public float AttackRange { get; set; }
    public float MoveSpeed { get; set; }
    public override int AttackDamage { get; set; }
}
