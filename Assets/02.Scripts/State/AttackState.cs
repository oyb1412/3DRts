public class AttackState : IUnitState
{
    public void OnUpdate(PlayerUnitBase unit)
    {
        if (unit.LockTarget == null)
        {
            unit.SetState(new IdleState());
            return;
        }
        
        unit.transform.LookAt(unit.LockTarget.transform.position);
    }
}
