using System.Collections.Generic;
using UnityEngine;

public class BuildingUIBehavior : IMiddleUIBehavior
{
    public void UpdateUI(Dictionary<string, GameObject> panels, List<IAllUnit> units)
    {
        foreach (var t in panels)
        {
            t.Value.gameObject.SetActive(false);
        }
        panels[UIMiddleStateController.Panels.BuildingStatePanel.ToString()].gameObject.SetActive(true);
        panels[UIMiddleStateController.Panels.BuildingStatePanel.ToString()].GetComponent<UIMiddleBuilding>().SetUI(units);
    }
}
