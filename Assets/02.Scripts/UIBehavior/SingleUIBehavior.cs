using System.Collections.Generic;
using UnityEngine;

public class SingleUIBehavior : IMiddleUIBehavior
{
    public void UpdateUI(Dictionary<string, GameObject> panels, List<IAllUnit> units)
    {
        foreach (var t in panels)
        {
            t.Value.gameObject.SetActive(false);
        }
        panels[UIMiddleStateController.Panels.SingleStatePanel.ToString()].gameObject.SetActive(true);
        panels[UIMiddleStateController.Panels.SingleStatePanel.ToString()].GetComponent<UIMiddleSingle>().SetUI(units);
    }
}
