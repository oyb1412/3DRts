using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIBuild : UIBase
{
    private enum BuildBtns
    {
        BarrackBtn,
        CastleBtn,
        TowerBtn,
        CanselBtn,
    }
    
    private List<Button> _buildBtns = new List<Button>();

    private void Start()
    {
        Bind<Button>(typeof(BuildBtns));

        for (int i = 0; i < Enum.GetValues(typeof(BuildBtns)).Length; i++)
        {
            _buildBtns.Add(Get<Button>(i).GetComponent<Button>());
        }
        
        _buildBtns[(int)BuildBtns.BarrackBtn].onClick.AddListener(() => Managers.Build.BuildShadow(Define.BuildList.Barrack));
        //_buildBtns[(int)BuildBtns.CastleBtn].onClick.AddListener(() => Managers.Instance.UnitController.SetState(UnitController.State.ClickQ));
        //_buildBtns[(int)BuildBtns.TowerBtn].onClick.AddListener(() => Managers.Instance.UnitController.SetState(UnitController.State.ClickS));
        _buildBtns[(int)BuildBtns.CanselBtn].onClick.AddListener(() => Managers.Instance.UIBehaviourPanel.HideUI());
    }
}
