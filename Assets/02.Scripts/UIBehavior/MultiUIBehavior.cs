using System.Collections.Generic;
using UnityEngine;

public class MultiUIBehavior : IMiddleUIBehavior
{
    public void UpdateUI(Dictionary<string, GameObject> panels, List<IAllUnit> units)
    {
        foreach (var t in panels)
        {
            t.Value.gameObject.SetActive(false);
        }
        panels[UIMiddleStateController.Panels.MultiStatePanel.ToString()].gameObject.SetActive(true);
        panels[UIMiddleStateController.Panels.MultiStatePanel.ToString()].GetComponent<UiMiddleMulti>().SetUI(units);

    }
    
    
}
