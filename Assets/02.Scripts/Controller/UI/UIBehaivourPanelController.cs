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

    private Panels _myPanels;
    private Dictionary<string, GameObject> _panels = new Dictionary<string, GameObject>();

    private void Start()
    {
        Bind<GameObject>(typeof(Panels));
        int count = 0;
        foreach (var value in Enum.GetValues(typeof(Panels)))
        {
           string panelName = Enum.GetName(typeof(Panels), value);
           _panels.Add(panelName, Get(count));
           _panels[panelName].gameObject.SetActive(false);
           count++;
        }
        
        Managers.Instance.UnitController.BehaviourUIEvent += SetBehaviourBtn;
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
