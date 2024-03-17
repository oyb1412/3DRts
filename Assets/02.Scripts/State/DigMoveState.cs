    using UnityEngine;

    public class DigMoveState : IUnitState
    {
        public void OnUpdate(PlayerUnitBase unit)
        {
            if (!unit.LockTarget)
            {
                unit.SetState(new IdleState());
                return;
            }
            

            
            unit.DestPos.y = unit.transform.position.y;
            float targetDir = (unit.LockTarget.transform.position - unit.transform.position).magnitude;
            if (targetDir <= unit.LockTarget.GetComponent<MeshRenderer>().bounds.size.x)
            {
                unit.transform.LookAt(unit.LockTarget.transform.position);
                unit.Nma.enabled = false;
                unit.SetState(new DigState());
            }
            else
            {
                unit.Nma.SetDestination(unit.LockTarget.transform.position);
            }

        }
    }
