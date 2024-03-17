using UnityEngine;

public class BuildingBuildState : IBuildingState
{
    public void OnUpdate(BuildingBase unit)
    {
        unit.MyStatus.Hp += 5 * Time.deltaTime;
        unit.OnHpEvent?.Invoke(unit.MyStatus.Hp);

        if (unit.MyStatus.Hp < unit.MyStatus.MaxHp) 
            return;

        if (unit.MyType == BuildingBase.BuildingType.Tower)
            unit.BuildState = new BuildingHoldState();
        else
            unit.BuildState = new BuildingIdleState();
        
        unit._myWorker.SetState(new IdleState());
    }
}