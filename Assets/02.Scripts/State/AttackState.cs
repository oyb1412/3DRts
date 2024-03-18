public class AttackState : IUnitState
{
    public AttackState(PlayerUnitBase unit)
    {
        unit.Nma.SetDestination(unit.transform.position);
        unit.Anime.CrossFade("Attack", .2f);
    }

    public void OnUpdate(PlayerUnitBase unit)
    {
        if (unit.LockTarget == null)
        {
            unit.SetState(new IdleState(unit));
            return;
        }
        
        unit.transform.LookAt(unit.LockTarget.transform.position);
    }
}
