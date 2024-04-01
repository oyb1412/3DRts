using System.Collections.Generic;
using UnityEngine;

public class MultiUIBehavior : IMiddleUIBehavior
{
    public void UpdateUI(List<GameObject> panels, List<IAllUnit> units)
    {
        foreach (var t in panels)
        {
            t.SetActive(false);
        }
        panels[(int)UIMiddleStateController.Panels.MultiStatePanel].gameObject.SetActive(true);
        panels[(int)UIMiddleStateController.Panels.MultiStatePanel].GetComponent<UiMiddleMulti>().SetUI(units);

    }
    
    
}
