using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleUIBehavior : IUIBehavior
{
    public void UpdateUI(Dictionary<string, GameObject> panels)
    {
        foreach (var t in panels)
        {
            t.Value.gameObject.SetActive(false);
        }
        panels[UIBehaviourPanelController.Panels.CastlePanel.ToString()].gameObject.SetActive(true);
    }
}
