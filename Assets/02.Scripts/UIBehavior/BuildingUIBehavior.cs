using System.Collections.Generic;
using UnityEngine;

public class BuildingUIBehavior : IMiddleUIBehavior
{
    public void UpdateUI(List<GameObject> panels, List<IAllUnit> units)
    {
        foreach (var t in panels)
        {
            t.SetActive(false);
        }
        panels[(int)UIMiddleStateController.Panels.BuildingStatePanel].gameObject.SetActive(true);
        panels[(int)UIMiddleStateController.Panels.BuildingStatePanel].GetComponent<UIMiddleBuilding>().SetUI(units);
    }
}
