using System;
using UnityEngine;

public class TowerController : BuildingBase
{
    public float ScanRange = 10f;
    public GameObject LockTarget;
    public AttackerBuildingStatus TowerStatus;
    protected override void Start()
    {
        TowerStatus = GetComponent<AttackerBuildingStatus>();
        TowerStatus.AttackRange = 10f;
        TowerStatus.AttackSpeed = 1f;
        TowerStatus.AttackDamage = 1;
        
        UIBehavior = new TowerUIBehavior();
    }

}
