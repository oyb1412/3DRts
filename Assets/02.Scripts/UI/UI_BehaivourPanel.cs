using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BehaivourPanel : UIBase
{
    public enum Buttons
    {
        MoveBtn,
        StopBtn,
        AttackBtn,
        HoldBtn,
        BuildBtn,
        BarrackBtn,
        CancleBtn,
    }
    
    public enum GameObjects
    {
        BehaivourPanel,
        BuildPanel,
    }

    private List<Button> behaivourBtns = new List<Button>();
    private GameObject _behaivourPanel;
    private GameObject _buildPanel;
    private void Start()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));
        _behaivourPanel = Get((int)GameObjects.BehaivourPanel);
        _buildPanel = Get((int)GameObjects.BuildPanel);
        _behaivourPanel.SetActive(true);
        _buildPanel.SetActive(false);
        for (int i = 0; i < Enum.GetValues(typeof(Buttons)).Length; i++)
        {
            behaivourBtns.Add(Get<Button>(i));
            behaivourBtns[i].gameObject.SetActive(false);
        }
        behaivourBtns[(int)Buttons.AttackBtn].onClick.AddListener(() => Managers.Instance.UnitController.SetState(UnitController.State.ClickA));
        behaivourBtns[(int)Buttons.MoveBtn].onClick.AddListener(() => Managers.Instance.UnitController.SetState(UnitController.State.ClickQ));
        behaivourBtns[(int)Buttons.StopBtn].onClick.AddListener(() => Managers.Instance.UnitController.SetState(UnitController.State.ClickS));
        behaivourBtns[(int)Buttons.HoldBtn].onClick.AddListener(() => Managers.Instance.UnitController.SetState(UnitController.State.ClickH));
        behaivourBtns[(int)Buttons.BuildBtn].onClick.AddListener(() => ChangedBuild(true));
        behaivourBtns[(int)Buttons.CancleBtn].onClick.AddListener(() => ChangedBuild(false));
        behaivourBtns[(int)Buttons.BarrackBtn].onClick.AddListener(() => Managers.Build.BuildShadow(Define.BuildList.Barrack));
        
        
        
        Managers.Instance.UnitController.behaivourUIEvent += SetBehaivourBtn;
    }

    private void ChangedBuild(bool doBuild)
    {
        _behaivourPanel.SetActive(!doBuild);
        _buildPanel.SetActive(doBuild);
        behaivourBtns[(int)Buttons.BarrackBtn].gameObject.SetActive(true);
        behaivourBtns[(int)Buttons.CancleBtn].gameObject.SetActive(true);
    }

    private void SetBehaivourBtn(IBehaviour type)
    {
        foreach (var item in behaivourBtns)
        {
            item.gameObject.SetActive(false);
        }
        _behaivourPanel.SetActive(true);
        _buildPanel.SetActive(false);
        if (type is IBuilder)
        {
            for (int i = 0; i < 5; i++)
            {
                behaivourBtns[i].gameObject.SetActive(true);
            }
            return;
        }

        if (type is IAttacker)
        {
            for (int i = 0; i < 4; i++)
            {
                behaivourBtns[i].gameObject.SetActive(true);
            }
        }
    }
    
    
}
