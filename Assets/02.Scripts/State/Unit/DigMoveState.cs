    using UnityEngine;

    namespace Unit.State
    {
        public class DigMoveState : IUnitState
        {
            public DigMoveState(PlayerUnitBase unit)
            {
                unit.Nma.enabled = true;
                unit.Anime.CrossFade("Move", .2f);
            }

            public void OnUpdate(PlayerUnitBase unit)
            {
                if (!unit.LockTarget)
                {
                    unit.SetState(new IdleState(unit));
                    return;
                }

                unit.DestPos.y = unit.transform.position.y;
                float targetDir = (unit.LockTarget.transform.position - unit.transform.position).magnitude;
                if (targetDir <= unit.LockTarget.GetComponent<MeshRenderer>().bounds.size.x)
                {
                    unit.transform.LookAt(unit.LockTarget.transform.position);
                    unit.Nma.enabled = false;
                    unit.SetState(new DigState(unit));
                }
                else
                {
                    unit.Nma.SetDestination(unit.LockTarget.transform.position);
                }

            }
        }
    }