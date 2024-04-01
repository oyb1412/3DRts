using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIMiddleStateController : UIBase
{
    public enum Panels
    {
        SingleStatePanel,
        MultiStatePanel,
        BuildingStatePanel,
    }

    private List<GameObject> _panels = new List<GameObject>();

    private void Start()
    {
        _panels.Add(Util.FindChild(gameObject, "SingleStatePanel"));
        _panels.Add(Util.FindChild(gameObject, "MultiStatePanel"));
        _panels.Add(Util.FindChild(gameObject, "BuildingStatePanel"));
        
        foreach(GameObject t in  _panels) {
            t.SetActive(false);
        }
        
        Managers.Instance.UnitController.MiddleBehaviourUIEvent += SetBehaviourBtn;
        Managers.Build.CancelBuild += HideUI;
    }

    public void ShowUI(Panels activePanel)
    {
        foreach (GameObject t in _panels) {
            t.SetActive(false);
        }

        _panels[(int)activePanel].SetActive(true);
    }
    
    public void HideUI()
    {
        foreach (GameObject t in _panels) {
            t.SetActive(false);
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
