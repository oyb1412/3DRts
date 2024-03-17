using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICastle : UIBase
{
    private enum CastleBtns
    {
       CreateWorkerBtn,
    }
    
    [SerializeField]private Button _castleBtns;

    private void Start()
    {
        Bind<Button>(typeof(CastleBtns));
        _castleBtns = Get<Button>((int)CastleBtns.CreateWorkerBtn).GetComponent<Button>();

        //todo
        //유닛 생성
        _castleBtns.onClick.AddListener(
            () => Managers.Instance.UnitController.SelectBuilding.GetComponent<IUnitProducer>().SetCreating((int)CastleBtns.CreateWorkerBtn));
        _castleBtns.onClick.AddListener(
            () => Debug.Log("111"));
    }
}
