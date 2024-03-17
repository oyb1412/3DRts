using UnityEngine;

public class BuildingHoldState : IBuildingState
{
    public void OnUpdate(BuildingBase unit)
    {
        TowerController tower = unit as TowerController;
        
        int mask = (1 << (int)Define.Layer.Monster);
        var monsters = Physics.OverlapSphere(tower.transform.position, tower.ScanRange, mask);
        var monster = Util.SortToShotDistance(monsters, tower.transform);
        if (monster != null)
        {
            tower.LockTarget = monster;
            tower.BuildState = new BuildingAttackState();
        }
    }
}