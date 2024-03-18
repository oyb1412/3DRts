using UnityEngine;

public class HoldState : IUnitState
{
    public HoldState(PlayerUnitBase unit)
    {
        unit.Nma.SetDestination(unit.transform.position);
        unit.Anime.CrossFade("Idle", .2f);
    }
    public void OnUpdate(PlayerUnitBase unit)
    {
        int mask = (1 << (int)Define.Layer.Monster);
        var monsters = Physics.OverlapSphere(unit.transform.position, unit.ScanRange, mask);
        var monster = Util.SortToShotDistance(monsters, unit.transform);
        if (monster != null)
        {
            unit.LockTarget = monster;
            unit.SetState(new MoveState(unit));
        }
    }
}