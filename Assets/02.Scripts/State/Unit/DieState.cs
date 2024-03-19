using UnityEngine;

namespace Unit.State
{
    public class DieState : IUnitState
    {
        public DieState(PlayerUnitBase unit)
        {
            unit.GetComponent<Collider>().enabled = false;
            unit.Nma.SetDestination(unit.transform.position);
            unit.Anime.CrossFade("Die", .2f);
        }

        public void OnUpdate(PlayerUnitBase unit)
        {
        }
    }
}
