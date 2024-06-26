using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIMiddleSingle : UIBase
{
    private enum SingleImages
    {
        UnitIconImage,
    }

    private enum SingleTexts
    {
        UnitHpText,
        UnitDamageText,
        UnitDefenseText,
        UnitNameText,
    }
    
    private Image _singleImages;
    private List<TextMeshProUGUI> _singleTexts = new List<TextMeshProUGUI>();
    private void Start()
    {
        _singleImages = Util.FindChild(gameObject, "UnitIconImage").GetComponent<Image>();

        _singleTexts.Add(Util.FindChild(gameObject, "UnitHpText").GetComponent<TextMeshProUGUI>());
        _singleTexts.Add(Util.FindChild(gameObject, "UnitDamageText").GetComponent<TextMeshProUGUI>());
        _singleTexts.Add(Util.FindChild(gameObject, "UnitDefenseText").GetComponent<TextMeshProUGUI>());
        _singleTexts.Add(Util.FindChild(gameObject, "UnitNameText").GetComponent<TextMeshProUGUI>());
    }

    private void SetHpEvent(IAllUnit unit)
    {
        PlayerUnitBase go = null;
        
        go = unit as PlayerUnitBase;

        if (!go)
            return;
        go.GetComponent<PlayerUnitBase>().OnHpEvent = null;
        go.GetComponent<PlayerUnitBase>().OnHpEvent += (hp =>
            _singleTexts[(int)SingleTexts.UnitHpText].text = $"{hp:FO}");
    }

    public void SetUI(List<IAllUnit> units)
    {
        if (units.Count == 0)
            return;
        
        //유닛의 값 가져오기
        PlayerUnitBase go = null;

        go = units[0] as PlayerUnitBase;

        if (!go)
            return;
        
        SetHpEvent(units[0]);
  
        //todo
        //아이콘 채우기
        
        UnitStatus baseStatus = go.Status;
        
        //체력 방어력 채우기
        _singleTexts[(int)SingleTexts.UnitHpText].text = baseStatus.Hp + " / " + baseStatus.MaxHp;
        _singleTexts[(int)SingleTexts.UnitDefenseText].text = baseStatus.Defense.ToString();
        
        //이름 채우기
        _singleTexts[(int)SingleTexts.UnitNameText].text = go.name;
        
        //유닛이 공격력을 지니고 있나 체크
        IAttackerStatus attacker = go.GetComponent<IAttackerStatus>();
        if (attacker != null)
        {
            //공격력 텍스트 채우기
            _singleTexts[(int)SingleTexts.UnitDamageText].text = attacker.AttackDamage.ToString();
            return;
        }
        //공격력 텍스트 비우기
        _singleTexts[(int)SingleTexts.UnitDamageText].text = string.Empty;
    }
}
