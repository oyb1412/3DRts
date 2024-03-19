using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private int _currentGold;
    private int _maxGold;
    public Action<int> OnGoldEvent;
    private List<GameObject> _allUnitList = new List<GameObject>();
    public int CurrentGold
    {
        get { return _currentGold; }
        set { _currentGold = value; }
    }

    public void CreateUnitSaveToAllUnitList(GameObject go)
    {
        if(go.GetComponent<Mark>())
            _allUnitList.Add(go);
    }
    
    public List<GameObject> GetAllUnitList()
    {
        return _allUnitList;
    }
    public int MaxGold
    {
        get { return _maxGold; }
        set { _maxGold = value; }
    }

    public void Init()
    {
        _currentGold = 500;
        _maxGold = 10000;
    }

    public void SetGoldEvent(int gold)
    {
        CurrentGold -= gold;
        OnGoldEvent?.Invoke(CurrentGold);
    }
}
