using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIAttacker : UIBase
{
    private enum AttackerBtns
    {
        MoveBtn,
        StopBtn,
        AttackBtn,
        HoldBtn,
    }
    
    private List<Button> _attackBtns = new List<Button>();

    private void Start()
    {
        _attackBtns.Add(Util.FindChild(gameObject, "MoveBtn").GetComponent<Button>());
        _attackBtns.Add(Util.FindChild(gameObject, "StopBtn").GetComponent<Button>());
        _attackBtns.Add(Util.FindChild(gameObject, "AttackBtn").GetComponent<Button>());
        _attackBtns.Add(Util.FindChild(gameObject, "HoldBtn").GetComponent<Button>());

        _attackBtns[(int)AttackerBtns.AttackBtn].onClick.AddListener(() => Managers.Instance.UnitController.SetState(UnitController.State.ClickA));
        _attackBtns[(int)AttackerBtns.MoveBtn].onClick.AddListener(() => Managers.Instance.UnitController.SetState(UnitController.State.ClickQ));
        _attackBtns[(int)AttackerBtns.StopBtn].onClick.AddListener(() => Managers.Instance.UnitController.SetState(UnitController.State.ClickS));
        _attackBtns[(int)AttackerBtns.HoldBtn].onClick.AddListener(() => Managers.Instance.UnitController.SetState(UnitController.State.ClickH));
    }
}
