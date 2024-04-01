using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleUIBehavior : IUIBehavior
{
    public void UpdateUI(List<GameObject> panels)
    {
        foreach (var t in panels)
        {
            t.SetActive(false);
        }
        panels[(int)UIBehaviourPanelController.Panels.CastlePanel].gameObject.SetActive(true);
    }
}
