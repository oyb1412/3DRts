using System;
using System.Collections.Generic;
using System.Linq;
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
    public Action<IUIBehavior> BehaviourUIEvent;
    public Action<IMiddleUIBehavior, List<IAllUnit>> MiddleBehaviourUIEvent;
    [SerializeField]private State _myState = State.None;
    
    private void Awake()
    {
        Managers.Input.OnMouseEvent += LeftClickEvent; 
        Managers.Input.OnMouseEvent += RightClickEvent;
        Managers.Input.OnMouseEvent += LeftAndAorQClickEvent;
        Managers.Input.OnKeyboardEvent += OnUpdateState;
    }

    public void SetState(State state)
    {
        _myState = state;
        
        switch (state)
        {
            case State.ClickH:
                SetAllUnitHoldState();
                break;
            case State.ClickS:
                SetAllUnitIdleState();
                break;
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
    private void LeftClickEvent(Define.MouseEventType type)
    {
        if (_myState is State.ClickA or State.ClickQ)
            return;

        if (Managers.Build.CurrentBuilding)
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
                    BuildingBase building = hit.collider.GetComponent<BuildingBase>();
                    if (player)
                    {
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
                    
                    if (building)
                    {
                        DeSelectedBuilding();
                        DeSelectedUnit();
                        SelectedBuilding(hit);

                    }
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
    
    private void RightClickEvent(Define.MouseEventType type)
    {       
        if (SelectUnit.Count == 0)
            return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position,ray.direction * 100f,Color.red,1f);
        int mask =  (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster) | (1 << (int)Define.Layer.Mine);
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
                    player.PlayerUnitMove(targetPositionList[targetPositionListIndex], new Unit.State.MoveState(player));
                    targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
                }

                break;
            }
            case (int)Define.Layer.Monster:
            {
                foreach (PlayerUnitBase player in SelectUnit)
                {
                    player.PlayerUnitAttack(hit.collider.gameObject, new Unit.State.MoveState(player));
                }

                break;
            }
            case (int)Define.Layer.Mine:
            {
                //todo
                //선택중인 유닛중 워커가 0명이면 리턴
                var worker = SelectUnit.FindAll(x => x.MyType == PlayerUnitBase.Type.Worker);
                if (worker.Count == 0)
                    return;

                foreach (var t in worker)
                {
                    t.LockTarget = hit.collider.gameObject;
                    t.SetState(new Unit.State.DigMoveState(t));
                }
            }
                break;
        }
        _myState = State.None;
    }

 
    
    private void LeftAndAorQClickEvent(Define.MouseEventType type)
    {
        if (SelectUnit.Count == 0)
            return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int mask =  (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
        bool rayCastHit = Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, mask);
        if (!rayCastHit || type != Define.MouseEventType.LeftClick) return;
        switch (_myState)
        {
            case State.ClickA:
            {
                if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                {
                    foreach (PlayerUnitBase player in SelectUnit)
                    {
                        player.PlayerUnitAttack(hit.collider.gameObject, new Unit.State.MoveState(player));
                    }
                }
                else if (hit.collider.gameObject.layer == (int)Define.Layer.Ground)
                {
                    List<Vector3> targetPositionList =
                        GetPositionListAround(hit.point, new float[] { 3f, 6f, 9f }, new int[] { 5, 10, 20 });
                    int targetPositionListIndex = 0;
                    foreach (PlayerUnitBase player in SelectUnit)
                    {
                        player.PlayerUnitMove(targetPositionList[targetPositionListIndex], new Unit.State.PatrolState(player));
                        targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
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
                        player.PlayerUnitMove(targetPositionList[targetPositionListIndex], new Unit.State.MoveState(player));
                        targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
                    }
                }

                break;
            }
        }
        _myState = State.None;
    }

    private void SelectedBuilding(RaycastHit hit)
    {
        BuildingBase building = hit.collider.GetComponent<BuildingBase>();
        building.SetSelectedState(Define.Select.Select);
        SelectBuilding = building;
        CheckAllUnitType();
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

    private void CheckAllUnitType()
    {
        if (SelectBuilding != null)
        {
            BehaviourUIEvent?.Invoke(SelectBuilding);
            MiddleBehaviourUIEvent?.Invoke(new BuildingUIBehavior(),
                new List<IAllUnit>()
                {
                    SelectBuilding
                });
            return;
        }
        
        if (SelectUnit.Count == 0)
        {
            BehaviourUIEvent?.Invoke(null);
            MiddleBehaviourUIEvent?.Invoke(null, null);
            return;
        }

        int count = 0;
        foreach (PlayerUnitBase unit in SelectUnit)
        {
            count++;
        }

        if (count > 1)
            MiddleBehaviourUIEvent?.Invoke(new MultiUIBehavior(), new List<IAllUnit>(SelectUnit));
        else
            MiddleBehaviourUIEvent?.Invoke(new SingleUIBehavior(), new List<IAllUnit>(SelectUnit));
        
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
    private void DeSelectedBuilding()
    {
        if (SelectBuilding == null)
            return;
        
        SelectBuilding.SetSelectedState(Define.Select.Deselect);

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
    
    private void SetAllUnitHoldState()
    {
        if (SelectUnit.Count == 0)
            return;

        foreach (PlayerUnitBase player in SelectUnit)
        {
            if(player.CurrentState is Unit.State.BuildState)
                continue;
            
            player.SetState(new Unit.State.HoldState(player));
        }
        CheckAllUnitType();
    }
    
    private void SetAllUnitIdleState()
    {
        if (SelectUnit.Count == 0)
            return;

        foreach (PlayerUnitBase player in SelectUnit)
        {
            if(player.CurrentState is Unit.State.BuildState)
                continue;
            
            player.SetState(new Unit.State.IdleState(player));
        }
        CheckAllUnitType();
    }
}
