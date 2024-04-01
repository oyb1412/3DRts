using UnityEngine;

namespace Building.State
{
    public class BuildState : IBuildingState
    {
        public void OnUpdate(BuildingBase unit)
        {
            unit.MyStatus.CurrentBuildingTime += Time.deltaTime;
            unit.OnBuildEvent?.Invoke(unit.MyStatus.CurrentBuildingTime / unit.MyStatus.MaxBuildingTime);

            if (unit.MyStatus.CurrentBuildingTime < unit.MyStatus.MaxBuildingTime)
                return;

            if (unit.MyType == BuildingBase.BuildingType.Tower)
                unit.BuildState = new HoldState();
            else
                unit.BuildState = new IdleState();

            unit.OnBuildCompleteEvent?.Invoke();
            unit.Worker.SetState(new Unit.State.IdleState(unit.Worker));
        }
    }
}