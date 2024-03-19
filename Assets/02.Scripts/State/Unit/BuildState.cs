namespace Unit.State
{
    public class BuildState : IUnitState
    {
        public BuildState(PlayerUnitBase unit)
        {
            unit.Nma.SetDestination(unit.transform.position);
            unit.Anime.CrossFade("Build", .2f);
        }

        public void OnUpdate(PlayerUnitBase unit)
        {

        }
    }
}
