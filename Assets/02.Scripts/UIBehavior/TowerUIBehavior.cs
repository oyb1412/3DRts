using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerUIBehavior : IUIBehavior
{
    public void UpdateUI(List<GameObject> panels)
    {
        foreach (var t in panels)
        {
            t.SetActive(false);
        }
        panels[(int)UIBehaviourPanelController.Panels.TowerPanel].gameObject.SetActive(true);
    }
}
