using System.Collections.Generic;
using UnityEngine;

public class BuildingCreateState : IBuildingState
{
    private Queue<int> _creatingUnitNumbers =new Queue<int>();
    public BuildingCreateState(int index)
    {
        _creatingUnitNumbers.Enqueue(index);
    }
    
    public void SetQueue(int index)
    {
        
        _creatingUnitNumbers.Enqueue(index);
    }
    public void OnUpdate(BuildingBase unit)
    {
        var building = unit.GetComponent<IUnitProducer>();

        if (_creatingUnitNumbers.Count == 0)
        {
            building.CurrentCreateTime = 0;
            return;
        }
        building.CurrentCreateTime += Time.deltaTime;
        unit.OnCreateSliderEvent?.Invoke(building.CurrentCreateTime / building.MaxCreateTime);
        unit.OnCreateImageEvent?.Invoke(building.CurrentCreateNumber);
        while (_creatingUnitNumbers.Count > 0 &&
               building.CurrentCreateTime >= building.MaxCreateTime)
        {
            int count = _creatingUnitNumbers.Dequeue();
            foreach (Define.UnitCreatePos t in building.Bound)
            {
                int mask = (1 << (int)Define.Layer.Player) | (1 << (int)Define.Layer.Monster) | (1 << (int)Define.Layer.Building) |  (1 << (int)Define.Layer.Mine);
                var rayCastHit = Physics.OverlapSphere(new Vector3(t.X, 2f, t.Z), 1f, mask);
                if (rayCastHit.Length == 0)
                {
                    GameObject unitObj = Managers.Resources.Activation($"Unit/{building.Units[count].name}", null);
                    unitObj.transform.position = new Vector3(t.X, 2f, t.Z);
                    building.CurrentCreateTime = 0;
                    building.CurrentCreateNumber--;
                    break;
                }
            }
        }

        if (building.CurrentCreateNumber == 0)
        {
            unit.OnCreateCompleteEvent?.Invoke();
            unit.BuildState = new BuildingIdleState();
        }

    }
}