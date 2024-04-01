using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBehaviourPanelController : UIBase
{
    public enum Panels
    {
        AttackerPanel,
        WorkerPanel,
        BuildPanel,
        BarrackPanel,
        CastlePanel,
        TowerPanel,
    }

    private List<GameObject> _panels = new List<GameObject>();

    private void Start()
    {
        _panels.Add(Util.FindChild(gameObject, "AttackerPanel"));
        _panels.Add(Util.FindChild(gameObject, "WorkerPanel"));
        _panels.Add(Util.FindChild(gameObject, "BuildPanel"));
        _panels.Add(Util.FindChild(gameObject, "BarrackPanel"));
        _panels.Add(Util.FindChild(gameObject, "CastlePanel"));
        _panels.Add(Util.FindChild(gameObject, "TowerPanel"));

        foreach (GameObject t in _panels) {
            t.SetActive(false);
        }

        Managers.Instance.UnitController.BehaviourUIEvent += SetBehaviourBtn;
        Managers.Build.CancelBuild += HideUI;
    }

    public void ShowUI(Panels activePanel)
    {
        foreach (GameObject t in _panels)
        {
            t.SetActive(false);
        }
        
        _panels[(int)activePanel].SetActive(true);
    }
    
    public void HideUI()
    {
        foreach (GameObject t in _panels)
        {
            t.SetActive(false);
        }
    }

    private void SetBehaviourBtn(IUIBehavior obj)
    {
        if (obj == null)
        {
            HideUI();
            return;
        }
        
        obj.UpdateUI(_panels);
    }

    private void SetButtonImageAlpha(GameObject go, bool trigger)
    {
        go.GetComponent<Image>().color = trigger ? 
            new Color(1f, 1f, 1f, 1f) : new Color(1f, 1f, 1f, 0f);
    }
}
