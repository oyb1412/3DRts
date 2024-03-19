using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIBarrack : UIBase
{
    private enum AttackerBtns
    {
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
        
        _barrackBtns[(int)AttackerBtns.CreateSwordManBtn].onClick.AddListener(
            () => Managers.Instance.UnitController.SelectBuilding.GetComponent<CreatorBuildingBase>().SetCreating((int)AttackerBtns.CreateSwordManBtn));
        _barrackBtns[(int)AttackerBtns.CreateArcherBtn].onClick.AddListener(
            () => Managers.Instance.UnitController.SelectBuilding.GetComponent<CreatorBuildingBase>().SetCreating((int)AttackerBtns.CreateArcherBtn));
    }
}
