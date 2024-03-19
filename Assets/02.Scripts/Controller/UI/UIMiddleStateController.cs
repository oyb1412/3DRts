using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMiddleStateController : UIBase
{
    public enum Panels
    {
        SingleStatePanel,
        MultiStatePanel,
        BuildingStatePanel,
    }

    private Dictionary<string, GameObject> _panels = new Dictionary<string, GameObject>();

    private void Start()
    {
        Bind<GameObject>(typeof(Panels));
        int count = 0;
        foreach (var value in Enum.GetValues(typeof(Panels)))
        {
            string panelName = Enum.GetName(typeof(Panels), value);
            _panels.TryAdd(panelName, Get(count));
            _panels[panelName].gameObject.SetActive(false);
            count++;
        }

        var q = _panels;
        
        Managers.Instance.UnitController.MiddleBehaviourUIEvent += SetBehaviourBtn;
        Managers.Build.CancelBuild += HideUI;
    }

    public void ShowUI(Panels activePanel)
    {
        foreach (var t in _panels)
        {
            t.Value.gameObject.SetActive(false);
        }
        
        _panels[activePanel.ToString()].gameObject.SetActive(true);
    }
    
    public void HideUI()
    {
        foreach (var t in _panels)
        {
            t.Value.gameObject.SetActive(false);
        }
    }

    private void SetBehaviourBtn(IMiddleUIBehavior obj, List<IAllUnit> units)
    {
        if (obj == null)
        {
            HideUI();
            return;
        }
        
        obj.UpdateUI(_panels, units);
    }
}
