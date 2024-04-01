using Building.State;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMiddleBuilding : UIBase
{
    private enum BuildingImages
    {
        BuildingIconImage,
        CreatingUnitImage0,
        CreatingUnitImage1,
        CreatingUnitImage2,
        CreatingUnitImage3,
        CreatingUnitImage4,
    }

    private enum BuildingTexts
    {
        BuildingHpText,
        BuildingNameText,
        BuildingAttackText,
        BuildingDefenseText,
    }

    private enum BuildingSliders
    {
        BuildingSlider,
    }
    
    private List<Image> _singleImages = new List<Image>();
    private Slider _singleSliders;
    private List<TextMeshProUGUI> _singleTexts = new List<TextMeshProUGUI>();
    private CreatorBuildingBase _selectedBuilding;

    private void Awake()
    {
        _singleSliders = Util.FindChild(gameObject, "BuildingSlider").GetComponent<Slider>();

        _singleImages.Add(Util.FindChild(gameObject, "BuildingIconImage").GetComponent<Image>());
        _singleImages.Add(Util.FindChild(gameObject, "CreatingUnitImage0").GetComponent<Image>());
        _singleImages.Add(Util.FindChild(gameObject, "CreatingUnitImage1").GetComponent<Image>());
        _singleImages.Add(Util.FindChild(gameObject, "CreatingUnitImage2").GetComponent<Image>());
        _singleImages.Add(Util.FindChild(gameObject, "CreatingUnitImage3").GetComponent<Image>());
        _singleImages.Add(Util.FindChild(gameObject, "CreatingUnitImage4").GetComponent<Image>());

        _singleTexts.Add(Util.FindChild(gameObject, "BuildingHpText").GetComponent<TextMeshProUGUI>());
        _singleTexts.Add(Util.FindChild(gameObject, "BuildingNameText").GetComponent<TextMeshProUGUI>());
        _singleTexts.Add(Util.FindChild(gameObject, "BuildingAttackText").GetComponent<TextMeshProUGUI>());
        _singleTexts.Add(Util.FindChild(gameObject, "BuildingDefenseText").GetComponent<TextMeshProUGUI>());
    }
    private void SetEvent(IAllUnit unit)
    {
        CreatorBuildingBase building = unit as CreatorBuildingBase;
        
        if (!building)
            return;

        if(building.BuildState is BuildState or CreateState) {
            _singleSliders.gameObject.SetActive(true);
            building.OnCreateImageEvent += SetCreateImageEvent;

        }
        else {
            _singleSliders.gameObject.SetActive(false);
        }
        

        building.OnBuildEvent += (amount => _singleSliders.value = amount);
        building.OnCreateCompleteEvent += () => _singleSliders.gameObject.SetActive(false);
        building.OnCreateSliderEvent += (amount => _singleSliders.value = amount);
        building.OnCreateStartEvent +=
            () => _singleSliders.gameObject.SetActive(true);
        building.OnHpEvent += (hp =>
            _singleTexts[(int)BuildingTexts.BuildingHpText].text = $"{hp:FO}");

        if (unit is BuildingBase) {
           BuildingBase build = unit as BuildingBase;
            build.OnBuildCompleteEvent += () => _singleSliders.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        for (int i = 1; i < _singleImages.Count; i++)
        {
            _singleImages[i].gameObject.SetActive(false);
        }

        if(_singleSliders != null)
            _singleSliders.value = 0f;

    }

    private void SetCreateImageEvent(int count)
    {
        for (int i = 1; i < _singleImages.Count; i++)
        {
            _singleImages[i].gameObject.SetActive(false);
        }

        for (int i = 1; i < count + 1; i++)
        {
            _singleImages[i].gameObject.SetActive(true);
        }
    }
    public void SetUI(List<IAllUnit> units)
    {
        if (units.Count == 0)
            return;
        
        //유닛의 값 가져오기
        BuildingBase go = null;

        go = units[0] as BuildingBase;

        if (!go)
            return;

        SetEvent(units[0]);
  
        //todo
        //아이콘 채우기
        
        BuildingStatus baseStatus = go.MyStatus;
        
        //체력 방어력 채우기
        _singleTexts[(int)BuildingTexts.BuildingHpText].text = baseStatus.Hp + " / " + baseStatus.MaxHp;
        _singleTexts[(int)BuildingTexts.BuildingDefenseText].text = baseStatus.Defense.ToString();
        
        //이름 채우기
        _singleTexts[(int)BuildingTexts.BuildingNameText].text = go.name;
        
        //유닛이 공격력을 지니고 있나 체크
        IAttackerStatus attacker = go.GetComponent<IAttackerStatus>();
        if (attacker != null)
        {
            //공격력 텍스트 채우기
            _singleTexts[(int)BuildingTexts.BuildingAttackText].text = attacker.AttackDamage.ToString();
            return;
        }
        //공격력 텍스트 비우기
        _singleTexts[(int)BuildingTexts.BuildingAttackText].text = string.Empty;
    }
}
