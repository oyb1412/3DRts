using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class UiMiddleMulti : UIBase
{
    private enum MiddleImages
    {
        UnitIconImage0,
        UnitIconImage1,
        UnitIconImage2,
        UnitIconImage3,
        UnitIconImage4,
        UnitIconImage5,
        UnitIconImage6,
        UnitIconImage7,
        UnitIconImage8,
        UnitIconImage9,
        UnitIconImage10,
        UnitIconImage11,
        Count,
    }

    
    private List<Image> _singleImages = new List<Image>();

    private void Start()
    {
        for(int i = 0; i< (int)MiddleImages.Count; i++) {
            _singleImages.Add(Util.FindChild(gameObject, $"UnitIconImage{i}").GetComponent<Image>());
        }
    }

    public void SetUI(List<IAllUnit> units)
    {
        if (units.Count == 0)
            return;
        
        //유닛의 값 가져오기
        List<PlayerUnitBase> go = new List<PlayerUnitBase>(units.Count);

        foreach (IAllUnit t in units)
        {
            go.Add(t as PlayerUnitBase);
        }
        
       
        
        foreach (var t in _singleImages)
        {
            t.gameObject.SetActive(false);
        }

        for (int i = 0; i < go.Count; i++)
        {
            _singleImages[i].gameObject.SetActive(true);
            BaseStatus baseStatus = go[i].GetComponent<BaseStatus>();
            //todo
            //아이콘 채우기
        }
 
    }
}
