using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIBuild : UIBase
{
    private enum BuildBtns
    {
        BarrackBtn,
        TowerBtn,
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
        _buildBtns[(int)BuildBtns.TowerBtn].onClick.AddListener(() => Managers.Build.BuildShadow(Define.BuildList.Tower));
    }
}
