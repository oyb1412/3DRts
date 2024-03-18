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
    private List<Slider> _singleSliders = new List<Slider>();
    private List<TextMeshProUGUI> _singleTexts = new List<TextMeshProUGUI>();

    private void Start()
    {
        Bind<Image>(typeof(BuildingImages));
        Bind<Slider>(typeof(BuildingSliders));
        Bind<TextMeshProUGUI>(typeof(BuildingTexts));

        for (int i = 0; i < Enum.GetValues(typeof(BuildingImages)).Length; i++)
        {
            _singleImages.Add(Get<Image>(i).GetComponent<Image>());
            _singleImages[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < Enum.GetValues(typeof(BuildingSliders)).Length; i++)
        {
            _singleSliders.Add(Get<Slider>(i).GetComponent<Slider>());
            _singleSliders[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < Enum.GetValues(typeof(BuildingTexts)).Length; i++)
        {
            _singleTexts.Add(Get<TextMeshProUGUI>(i).GetComponent<TextMeshProUGUI>());
        }
    }
    private void SetEvent(IAllUnit unit)
    {
        BuildingBase go = unit as BuildingBase;
        
        if (!go)
            return;
        
        BuildingBase building = go.GetComponent<BuildingBase>();
        
        building.OnHpEvent += (hp =>
            _singleTexts[(int)BuildingTexts.BuildingHpText].text = $"{hp:FO}");
        building.OnCreateSliderEvent += (amount => _singleSliders[(int)BuildingSliders.BuildingSlider].value = amount);
        building.OnCreateImageEvent += SetCreateImageEvent;
        
        building.OnCreateStartEvent +=
            () => _singleSliders[(int)BuildingSliders.BuildingSlider].gameObject.SetActive(true);
        building.OnBuildEvent += (amount => _singleSliders[(int)BuildingSliders.BuildingSlider].value = amount);
    }

    private void OnDisable()
    {
        _singleSliders[(int)BuildingSliders.BuildingSlider].value = 0f;
        for (int i = 1; i < _singleImages.Count; i++)
        {
            _singleImages[i].gameObject.SetActive(false);
        }
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
