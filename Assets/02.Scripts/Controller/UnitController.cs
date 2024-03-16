using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public enum State
    {
        None,
        ClickS,
        ClickA,
        ClickH,
        ClickQ,
        ClickB,
    }
    public List<PlayerUnitBase> SelectUnit = new List<PlayerUnitBase>();
    public BuildingBase SelectBuilding;
    private PlayerUnitBase _lastSelectUnit;
    private PlayerUnitBase _lastSelectEnemy;
    public Action<IUIBehavior> BehaviourUIEvent;
    private State _myState = State.None;
    
    private void Start()
    {
        Managers.Input.OnMouseEvent += UnitSelectEvent; 
        Managers.Input.OnMouseEvent += UnitAttackAndMoveEvent;
        Managers.Input.OnMouseEvent += UnitAClickAttackAndMoveEvent;
        Managers.Input.OnKeyboardEvent += OnUpdateState;
    }

    public void SetState(State state)
    {
        _myState = state;
        if (state == State.ClickH)
        {
            SetAllUnitState(new HoldState());
        }

        if (state == State.ClickS)
        {
            SetAllUnitState(new IdleState());
        }
    }
    private void OnUpdateState()
    {
        if (Input.GetKeyDown(KeyCode.S))
            SetState(State.ClickS);
        if (Input.GetKeyDown(KeyCode.A))
            SetState(State.ClickA);
        if (Input.GetKeyDown(KeyCode.H))
            SetState(State.ClickH);
        if (Input.GetKeyDown(KeyCode.Q))
            SetState(State.ClickQ);
        if (Input.GetKeyDown(KeyCode.B))
            SetState(State.ClickB);
    }
    private void UnitSelectEvent(Define.MouseEventType type)
    {
        if (_myState is State.ClickA or State.ClickQ)
            return;


        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int mask = (1 << (int)Define.Layer.Player) | (1 << (int)Define.Layer.Monster) | (1 << (int)Define.Layer.Building);
        bool rayCastHit = Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, mask);

        switch (type)
        {
            case Define.MouseEventType.LeftClick:
            {
                if (rayCastHit)
                {
                    PlayerUnitBase player = hit.collider.GetComponent<PlayerUnitBase>();
                    PlayerUnitBase enemy = hit.collider.GetComponent<PlayerUnitBase>();
                    BuildingBase building = hit.collider.GetComponent<BuildingBase>();
                    if (player)
                    {
                        if (_lastSelectEnemy != null)
                        {
                            _lastSelectEnemy.MySelect = Define.Select.Deselect;
                            _lastSelectEnemy = null;
                            SelectedUnit(hit);
                            DeSelectedBuilding();
                        }
                        //마지막으로 선택한 유닛이 존재하고
                        //마지막으로 선택한 유닛과 새롭게 선택한 유닛의 종류가 같다면,
                        if (_lastSelectUnit != null && player == _lastSelectUnit)
                        {
                            //일정 범위내의 그 종류의 유닛을 모두 선택한다.
                            PlayerUnitBase[] players = Util.SelectedAutoUnits(_lastSelectUnit, _lastSelectUnit.MyType, 30f, 12);
                            SelectedUnits(players);
                            DeSelectedBuilding();
                            _lastSelectUnit = null;
                            return;
                        }
                        //마지막으로 선택한 유닛을 저장한다.
                        _lastSelectUnit = player;
                        //아무도 선택되어있지 않다면 새롭게 선택한다.
                        if (SelectUnit.Count == 0)
                        {
                            SelectedUnit(hit);
                            DeSelectedBuilding();
                        }
                        //누군가 선택되어 있다면 바꿔친다.
                        else
                        {
                            DeSelectedUnit();
                            SelectedUnit(hit);
                            DeSelectedBuilding();
                        }
                        return;
                    }
                    if (enemy)
                    {
                        DeSelectedUnit();
                        DeSelectedBuilding();
                        _lastSelectEnemy = enemy;
                        _lastSelectEnemy.MySelect = Define.Select.Select;
                        return;
                    }
                    if (building)
                    {
                        SelectedBuilding(hit);
                        DeSelectedUnit();
                    }
                }
                else if(_lastSelectEnemy != null)
                {
                    _lastSelectEnemy.MySelect = Define.Select.Deselect;
                    _lastSelectEnemy = null;
                    DeSelectedUnit();
                    DeSelectedBuilding();
                }
                else
                {
                    DeSelectedUnit();
                    DeSelectedBuilding();
                }
            }
                break;
        }
    }

    private void SelectedBuilding(RaycastHit hit)
    {
        BuildingBase building = hit.collider.GetComponent<BuildingBase>();
        building.MySelect = Define.Select.Select;
        SelectBuilding = building;
        CheckAllUnitType();
    }
    
    private void DeSelectedBuilding()
    {
        if (SelectBuilding == null)
            return;
        SelectBuilding.MySelect = Define.Select.Deselect;
        
        SelectBuilding = null;

        CheckAllUnitType();
    }
    private void DeSelectedUnit()
    {
        if (Managers.Build.CurrentBuilding)
            return;
        
        foreach (PlayerUnitBase player in SelectUnit)
        {
            player.MySelect = Define.Select.Deselect;
        }
        SelectUnit.Clear();
        CheckAllUnitType();
    }

    private void SelectedUnit(RaycastHit hit)
    {
        if (Managers.Build.CurrentBuilding)
            return;
        
        PlayerUnitBase player = hit.collider.GetComponent<PlayerUnitBase>();
        player.MySelect = Define.Select.Select;
        SelectUnit.Add(player);
        CheckAllUnitType();
    }
    
    public void SelectedUnit(GameObject go)
    {
        if (Managers.Build.CurrentBuilding)
            return;
        
        PlayerUnitBase player = go.GetComponent<PlayerUnitBase>();
        player.MySelect = Define.Select.Select;
        SelectUnit.Add(player);
        CheckAllUnitType();
    }
    
    private void SelectedUnits(PlayerUnitBase[] player)
    {
        if (Managers.Build.CurrentBuilding)
            return;
        
        if (player.Length <= 0)
            return;
        
        foreach (PlayerUnitBase t in player)
        {
            if (t == null)
                break;
                
            t.MySelect = Define.Select.Select;
            SelectUnit.Add(t);
        }

        CheckAllUnitType();
    }
    
    private void UnitAttackAndMoveEvent(Define.MouseEventType type)
    {       
        if (SelectUnit.Count == 0 || _myState == State.ClickA)
            return;
        
        
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position,ray.direction * 100f,Color.red,1f);
        int mask =  (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
        bool rayCastHit = Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, mask);
        if (!rayCastHit || type != Define.MouseEventType.RightClick) return;
        switch (hit.collider.gameObject.layer)
        {
            case (int)Define.Layer.Ground:
            {
                List<Vector3> targetPositionList = GetPositionListAround(hit.point, new float[] {3f, 6f, 9f}, new int[] {5, 10, 20});
                int targetPositionListIndex = 0;
                foreach (PlayerUnitBase player in SelectUnit)
                {
                    player.PlayerUnitMove(targetPositionList[targetPositionListIndex], new MoveState());
                    targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
                }

                break;
            }
            case (int)Define.Layer.Monster:
            {
                foreach (PlayerUnitBase player in SelectUnit)
                {
                    player.PlayerUnitAttack(hit.collider.gameObject, new MoveState());
                }

                break;
            }
        }
    }

    private List<Vector3> GetPositionListAround(Vector3 startPosition, float distance, int positionCount)
    {
        List<Vector3> positionList = new List<Vector3>();
        for (int i = 0; i < positionCount; i++)
        {
            float angle = i * (360f / positionCount);
            Vector3 dir = ApplyRotationToVector(new Vector3(1, 0), angle);
            Vector3 position = startPosition + dir * distance;
            positionList.Add(position);
        }

        return positionList;
    }

    private List<Vector3> GetPositionListAround(Vector3 startPosition, float[] ringDistanceArray,int[] ringPositionCount)
    {
        List<Vector3> positionList = new List<Vector3> { startPosition };
        for (int i = 0; i < ringDistanceArray.Length; i++)
        {
            positionList.AddRange(GetPositionListAround(startPosition,ringDistanceArray[i],ringPositionCount[i]));
        }

        return positionList;
    }
    private Vector3 ApplyRotationToVector(Vector3 vec, float angle)
    {
        return Quaternion.Euler(0, angle, 0) * vec;
    }

    public void StartBuild(GameObject go)
    {
        GameObject building = go;
        foreach (var item in SelectUnit)
        {
            if (item.MyType == PlayerUnitBase.Type.Worker)
            {
                if(item.CurrentState is BuildState)
                    continue;
                
                item.GetComponent<WorkerController>().SetBuildState(building);
                break;
            }
        }
    }
    
    private void CheckAllUnitType()
    {
        if (SelectBuilding != null)
        {
            BehaviourUIEvent?.Invoke(SelectBuilding);
            return;
        }
        
        if (SelectUnit.Count == 0)
        {
            BehaviourUIEvent?.Invoke(null);
            return;
        }
        
        foreach (PlayerUnitBase unit in SelectUnit)
        {
            if (unit.GetComponent<IBuilder>() != null)
            {
                BehaviourUIEvent?.Invoke(unit);
                return;
            }
        }
        foreach (PlayerUnitBase unit in SelectUnit)
        {
            if (unit.GetComponent<IAttacker>() != null)
            {
                BehaviourUIEvent?.Invoke(unit);
                return;
            }
        }
    }
    
    private void UnitAClickAttackAndMoveEvent(Define.MouseEventType type)
    {
        if (SelectUnit.Count == 0)
            return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int mask =  (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
        bool rayCastHit = Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, mask);
        if (!rayCastHit || type != Define.MouseEventType.LeftClick) return;
        switch (_myState)
        {
            case State.ClickA when hit.collider.gameObject.layer == (int)Define.Layer.Ground:
            {
                List<Vector3> targetPositionList =
                    GetPositionListAround(hit.point, new float[] { 3f, 6f, 9f }, new int[] { 5, 10, 20 });
                int targetPositionListIndex = 0;
                foreach (PlayerUnitBase player in SelectUnit)
                {
                    player.PlayerUnitMove(targetPositionList[targetPositionListIndex], new PatrolState());
                    targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
                }

                break;
            }
            case State.ClickA:
            {
                if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                {
                    foreach (PlayerUnitBase player in SelectUnit)
                    {
                        player.PlayerUnitAttack(hit.collider.gameObject, new MoveState());
                    }
                }

                break;
            }
            case State.ClickQ:
            {
                if (hit.collider.gameObject.layer == (int)Define.Layer.Ground)
                {
                    List<Vector3> targetPositionList =
                        GetPositionListAround(hit.point, new float[] { 3f, 6f, 9f }, new int[] { 5, 10, 20 });
                    int targetPositionListIndex = 0;
                    foreach (PlayerUnitBase player in SelectUnit)
                    {
                        player.PlayerUnitMove(targetPositionList[targetPositionListIndex], new MoveState());
                        targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
                    }
                }

                break;
            }
        }
        _myState = State.None;
    }

    private void SetAllUnitState(IUnitState state)
    {
        if (SelectUnit.Count == 0)
            return;

        foreach (PlayerUnitBase player in SelectUnit)
        {
            if(player.CurrentState is BuildState)
                continue;
            
            player.SetState(state);
        }

        CheckAllUnitType();
    }
}
