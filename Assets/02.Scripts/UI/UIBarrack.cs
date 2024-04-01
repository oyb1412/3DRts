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
        _barrackBtns.Add(Util.FindChild(gameObject, "CreateSwordManBtn").GetComponent<Button>());
        _barrackBtns.Add(Util.FindChild(gameObject, "CreateArcherBtn").GetComponent<Button>());

        _barrackBtns[(int)AttackerBtns.CreateSwordManBtn].onClick.AddListener(
            () => Managers.Instance.UnitController.SelectBuilding.GetComponent<CreatorBuildingBase>().SetCreating((int)AttackerBtns.CreateSwordManBtn));
        _barrackBtns[(int)AttackerBtns.CreateArcherBtn].onClick.AddListener(
            () => Managers.Instance.UnitController.SelectBuilding.GetComponent<CreatorBuildingBase>().SetCreating((int)AttackerBtns.CreateArcherBtn));
    }
}
