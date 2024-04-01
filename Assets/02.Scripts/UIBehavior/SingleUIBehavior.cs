using System.Collections.Generic;
using UnityEngine;

public class SingleUIBehavior : IMiddleUIBehavior
{
    public void UpdateUI(List<GameObject> panels, List<IAllUnit> units)
    {
        foreach (var t in panels)
        {
            t.SetActive(false);
        }
        panels[(int)UIMiddleStateController.Panels.SingleStatePanel].gameObject.SetActive(true);
        panels[(int)UIMiddleStateController.Panels.SingleStatePanel].GetComponent<UIMiddleSingle>().SetUI(units);
    }
}
