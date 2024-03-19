using UnityEngine;

namespace Building.State
{
    public class AttackState : IBuildingState
    {
        private float _timer;
        public void OnUpdate(BuildingBase unit)
        {
            TowerController tower = unit as TowerController;
            if (tower.LockTarget == null)
                tower.BuildState = new HoldState();
        
            //todo
            //타워 공격 구현
            _timer += Time.deltaTime;
            if (_timer > tower.TowerStatus.AttackSpeed)
            {
                ProjectileController arrow = Managers.Resources.Activation("ArrowParent", null).GetComponent<ProjectileController>();
                arrow.Init(tower.LockTarget, tower.transform.position, tower.TowerStatus);
                _timer = 0;
            }
        }
    }
}

