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
        _buildBtns.Add(Util.FindChild(gameObject, "BarrackBtn").GetComponent<Button>());
        _buildBtns.Add(Util.FindChild(gameObject, "TowerBtn").GetComponent<Button>());

        _buildBtns[(int)BuildBtns.BarrackBtn].onClick.AddListener(() => Managers.Build.BuildShadow(Define.BuildList.Barrack));
        _buildBtns[(int)BuildBtns.TowerBtn].onClick.AddListener(() => Managers.Build.BuildShadow(Define.BuildList.Tower));
    }
}
