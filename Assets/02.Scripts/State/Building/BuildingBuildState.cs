using UnityEngine;

public class BuildingBuildState : IBuildingState
{
    public void OnUpdate(BuildingBase unit)
    {
        unit.MyStatus.CurrentBuildingTime += Time.deltaTime;
        unit.OnBuildEvent?.Invoke(unit.MyStatus.CurrentBuildingTime / unit.MyStatus.MaxBuildingTime);

        if (unit.MyStatus.CurrentBuildingTime < unit.MyStatus.MaxBuildingTime) 
            return;

        if (unit.MyType == BuildingBase.BuildingType.Tower)
            unit.BuildState = new BuildingHoldState();
        else
            unit.BuildState = new BuildingIdleState();
        
        
        unit.Worker.SetState(new IdleState(unit.Worker));
    }
}