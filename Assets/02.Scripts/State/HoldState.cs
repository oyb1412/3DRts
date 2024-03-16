using UnityEngine;

public class HoldState : IUnitState
{
    public void OnUpdate(PlayerUnitBase unit)
    {
        int mask = (1 << (int)Define.Layer.Monster);
        var monsters = Physics.OverlapSphere(unit.transform.position, unit.ScanRange, mask);
        var monster = Util.SortToShotDistance(monsters, unit.transform);
        if (monster != null)
        {
            unit.LockTarget = monster;
            unit.SetState(new MoveState());
        }
    }
}