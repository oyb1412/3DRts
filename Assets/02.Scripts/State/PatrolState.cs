using UnityEngine;

public class PatrolState : IUnitState
{
    public void OnUpdate(PlayerUnitBase unit)
    {
        unit.DestPos.y = unit.transform.position.y;
        float dir = (unit.DestPos - unit.transform.position).magnitude;
        if (dir <= 0.1f)
        {
            unit.SetState(new IdleState());
        }
        else
        {
            unit.Nma.SetDestination(unit.DestPos);
            int mask = (1 << (int)Define.Layer.Monster);
            var monsters = Physics.OverlapSphere(unit.transform.position, unit.ScanRange, mask);
            var monster = Util.SortToShotDistance(monsters, unit.transform);
            if (monster != null)
            {
                unit.SetState(new MoveState());
                unit.LockTarget = monster;
            }
        }
    }
}