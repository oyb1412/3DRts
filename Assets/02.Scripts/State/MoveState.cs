public class MoveState : IUnitState
{
    public void OnUpdate(PlayerUnitBase unit)
    {
        if (unit.LockTarget)
        {
            unit.DestPos.y = unit.transform.position.y;
            float targetDir = (unit.LockTarget.transform.position - unit.transform.position).magnitude;
            if (targetDir <= unit.Status.AttackRange)
            {
                unit.SetState(new AttackState());
            }
            else
            {
                unit.Nma.SetDestination(unit.LockTarget.transform.position);
            }
        }
        else
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
            }
        }
    }
}
