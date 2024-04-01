using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarrackUIBehavior : IUIBehavior
{
    public void UpdateUI(List<GameObject> panels)
    {
        foreach (var t in panels)
        {
            t.SetActive(false);
        }
        panels[(int)UIBehaviourPanelController.Panels.BarrackPanel].gameObject.SetActive(true);
    }
}
