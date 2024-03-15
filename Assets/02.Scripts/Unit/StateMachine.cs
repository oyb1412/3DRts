using System.Collections.Generic;

public class StateMachine
{
    public void Init(PlayerUnitBase owner, IState initState)
    {
        _ownerPlayer = owner;
        _myState = initState;
    }
    public void Init(EnemyUnitBase owner, IState initState)
    {
        _ownerEnemy = owner;
        _myState = initState;
    }

    private PlayerUnitBase _ownerPlayer;
    private EnemyUnitBase _ownerEnemy;
    private IState _myState; 
    public void OnUpdate()
    {
        _myState.OnUpdateState();
    }

    public void OnChange(Define.State newState)
    {
        _myState.OnChangeState(newState);
    }
    
}
