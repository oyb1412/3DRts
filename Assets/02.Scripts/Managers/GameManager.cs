public class GameManager
{
    private int _currentGold;
    private int _maxGold;
    
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
}
