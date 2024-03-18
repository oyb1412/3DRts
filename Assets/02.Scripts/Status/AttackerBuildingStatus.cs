using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerBuildingStatus : BaseStatus , IAttackerStatus
{
    public float AttackSpeed { get; set; }
    public float AttackRange { get; set; }
    public int AttackDamage { get; set; }
}
