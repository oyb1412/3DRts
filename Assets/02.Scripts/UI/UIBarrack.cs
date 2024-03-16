using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIBarrack : UIBase
{
    private enum AttackerBtns
    {
       CreateWorkerBtn,
       CreateSwordManBtn,
       CreateArcherBtn,
    }
    
    private List<Button> _barrackBtns = new List<Button>();

    private void Start()
    {
        Bind<Button>(typeof(AttackerBtns));

        for (int i = 0; i < Enum.GetValues(typeof(AttackerBtns)).Length; i++)
        {
            _barrackBtns.Add(Get<Button>(i).GetComponent<Button>());
        }
        
        //todo
        //유닛 생성
        //_barrackBtns[(int)AttackerBtns.CreateWorker].onClick.AddListener(() => Managers.Instance.UnitController.SetState(UnitController.State.ClickA));
        //_barrackBtns[(int)AttackerBtns.CreateSwordMan].onClick.AddListener(() => Managers.Instance.UnitController.SetState(UnitController.State.ClickQ));
        //_barrackBtns[(int)AttackerBtns.CreateArcher].onClick.AddListener(() => Managers.Instance.UnitController.SetState(UnitController.State.ClickS));
    }
}
