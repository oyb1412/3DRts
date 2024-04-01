using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuilderUIBehavior : IUIBehavior
{
    public void UpdateUI(List<GameObject> panels)
    {
        foreach (var t in panels)
        {
            t.SetActive(false);
        }
        panels[(int)UIBehaviourPanelController.Panels.WorkerPanel].gameObject.SetActive(true);
        
    }
}
