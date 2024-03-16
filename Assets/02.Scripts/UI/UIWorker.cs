using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWorker : UIBase
{
    private enum WorkerBtns
    {
        MoveBtn,
        StopBtn,
        AttackBtn,
        HoldBtn,
        BuildBtn,
    }
    
    private List<Button> _workerBtns = new List<Button>();

    private void Start()
    {
        Bind<Button>(typeof(WorkerBtns));
        
        for (int i = 0; i < Enum.GetValues(typeof(WorkerBtns)).Length; i++)
        {
            _workerBtns.Add(Get<Button>(i).GetComponent<Button>());
        }
        
        _workerBtns[(int)WorkerBtns.AttackBtn].onClick.AddListener(() => Managers.Instance.UnitController.SetState(UnitController.State.ClickA));
        _workerBtns[(int)WorkerBtns.MoveBtn].onClick.AddListener(() => Managers.Instance.UnitController.SetState(UnitController.State.ClickQ));
        _workerBtns[(int)WorkerBtns.StopBtn].onClick.AddListener(() => Managers.Instance.UnitController.SetState(UnitController.State.ClickS));
        _workerBtns[(int)WorkerBtns.HoldBtn].onClick.AddListener(() => Managers.Instance.UnitController.SetState(UnitController.State.ClickH));
        _workerBtns[(int)WorkerBtns.BuildBtn].onClick.AddListener(() => Managers.Instance.UIBehaviourPanel.ShowUI(UIBehaviourPanelController.Panels.BuildPanel));
    }
}
