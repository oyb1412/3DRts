using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackerUIBehavior : IUIBehavior
{
    public void UpdateUI(List<GameObject> panels)
    {
        foreach (var t in panels)
        {
            t.SetActive(false);
        }
        panels[(int)UIBehaviourPanelController.Panels.AttackerPanel].gameObject.SetActive(true);
    }
}
