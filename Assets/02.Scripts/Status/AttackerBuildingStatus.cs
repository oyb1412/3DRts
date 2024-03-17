using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerBuildingStatus : BaseStatus
{
    public float AttackSpeed { get; set; }
    public float AttackRange { get; set; }
    public override int AttackDamage { get; set; }
}
