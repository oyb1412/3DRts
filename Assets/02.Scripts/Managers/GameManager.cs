using System;

public class GameManager
{
    private int _currentGold;
    private int _maxGold;
    public Action<int> OnGoldEvent;
    public int CurrentGold
    {
        get { return _currentGold; }
        set { _currentGold = value; }
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
