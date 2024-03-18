public class IdleState : IUnitState
{
    public IdleState(PlayerUnitBase unit)
    {
        unit.Nma.SetDestination(unit.transform.position);
        unit.Anime.CrossFade("Idle", .2f);
    }
    public void OnUpdate(PlayerUnitBase unit)
    {
    }
}
