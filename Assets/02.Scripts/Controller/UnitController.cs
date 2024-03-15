using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

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
    private List<PlayerUnitBase> _selectUnit = new List<PlayerUnitBase>();
    private PlayerUnitBase _lastSelectUnit;
    private EnemyUnitBase _lastSelectMonster;
    public Action<IBehaviour> behaivourUIEvent;
    [SerializeField]private State _myState = State.None;
    
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
            SetAllUnitState(Define.State.Hold);
        }

        if (state == State.ClickS)
        {
            SetAllUnitState(Define.State.Idle);
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
    public void UnitSelectEvent(Define.MouseEventType type)
    {
        if (_myState == State.ClickA || _myState == State.ClickQ)
            return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(Camera.main.transform.position,ray.direction * 100f,Color.red,1f);
        RaycastHit hit;
        int mask = (1 << (int)Define.Layer.Player) | (1 << (int)Define.Layer.Monster);
        bool raycastHit = Physics.Raycast(ray, out hit, float.MaxValue, mask);

        switch (type)
        {
            case Define.MouseEventType.LeftClick:
            {
                if (raycastHit)
                {
                    PlayerUnitBase player = hit.collider.GetComponent<PlayerUnitBase>();
                    EnemyUnitBase enemy = hit.collider.GetComponent<EnemyUnitBase>();
                    if (player)
                    {
                        if (_lastSelectMonster != null)
                        {
                            _lastSelectMonster.MySelect = EnemyUnitBase.Select.Deselect;
                            _lastSelectMonster = null;
                        }
                        //마지막으로 선택한 유닛이 존재하고
                        //마지막으로 선택한 유닛과 새롭게 선택한 유닛의 종류가 같다면,
                        if (_lastSelectUnit != null && player == _lastSelectUnit)
                        {
                            //일정 범위내의 그 종류의 유닛을 모두 선택한다.
                            PlayerUnitBase[] players = Util.SelectedAutoUnits(_lastSelectUnit, _lastSelectUnit.MyType, 30f, 12);
                            SelectedUnits(players);
                            _lastSelectUnit = null;
                            return;
                        }
                        else
                        {
                            //마지막으로 선택한 유닛을 저장한다.
                            _lastSelectUnit = player;
                        }
                        //아무도 선택되어있지 않다면 새롭게 선택한다.
                        if (_selectUnit.Count == 0)
                        {
                            SelectedUnit(hit);
                        }
                        //누군가 선택되어 있다면 바꿔친다.
                        else
                        {
                            DeSelectedUnit();
                            SelectedUnit(hit);
                        }
                        

                    }
                    else if (enemy)
                    {
                        DeSelectedUnit();
                        _lastSelectMonster = enemy;
                        _lastSelectMonster.MySelect = EnemyUnitBase.Select.Select;
                    }
                }
                else if(_lastSelectMonster != null)
                {
                    _lastSelectMonster.MySelect = EnemyUnitBase.Select.Deselect;
                    _lastSelectMonster = null;
                    DeSelectedUnit();
                }
                else
                    DeSelectedUnit();
            }
                break;
        }
    }

    private void DeSelectedUnit()
    {
        foreach (PlayerUnitBase player in _selectUnit)
        {
            player.MySelect = PlayerUnitBase.Select.Deselect;
        }
        _selectUnit.Clear();
        CheckAllUnitType();
    }

    private void SelectedUnit(RaycastHit hit)
    {
        PlayerUnitBase player = hit.collider.GetComponent<PlayerUnitBase>();
        player.MySelect = PlayerUnitBase.Select.Select;
        _selectUnit.Add(player);
        CheckAllUnitType();
    }
    
    public void SelectedUnit(GameObject go)
    {
        PlayerUnitBase player = go.GetComponent<PlayerUnitBase>();
        player.MySelect = PlayerUnitBase.Select.Select;
        _selectUnit.Add(player);
        CheckAllUnitType();
    }
    
    private void SelectedUnits(PlayerUnitBase[] player)
    {
        if (player.Length <= 0)
            return;
        
        for (int i = 0; i < player.Length; i++)
        {
            if (player[i] == null)
                return;
                
            player[i].MySelect = PlayerUnitBase.Select.Select;
            _selectUnit.Add(player[i]);
        }

        CheckAllUnitType();
    }
    
    private void UnitAttackAndMoveEvent(Define.MouseEventType type)
    {       
        if (_selectUnit.Count == 0 || _myState == State.ClickA)
            return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position,ray.direction * 100f,Color.red,1f);
        RaycastHit hit;
        int mask =  (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
        bool raycastHit = Physics.Raycast(ray, out hit, float.MaxValue, mask);
        if (raycastHit && type == Define.MouseEventType.RightClick)
        {
            if(hit.collider.gameObject.layer == (int)Define.Layer.Ground)
            {
                List<Vector3> targetPositionList = GetPositionListAround(hit.point, new float[] {3f, 6f, 9f}, new int[] {5, 10, 20});
                int targetPositionListIndex = 0;
                foreach (PlayerUnitBase player in _selectUnit)
                {
                    player.PlayerUnitMove(targetPositionList[targetPositionListIndex], Define.State.Move);
                    targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
                }
                
            }
            else if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {
                foreach (PlayerUnitBase player in _selectUnit)
                {
                    player.PlayerUnitAttack(Define.State.Move, hit.collider.gameObject);
                }
                
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
        List<Vector3> positionList = new List<Vector3>();
        positionList.Add(startPosition);
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
        int count = 0;
        if (_selectUnit.Count == 0)
        {
            behaivourUIEvent?.Invoke(null);
            return;
        }
        foreach (PlayerUnitBase unit in _selectUnit)
        {
            if (unit.GetComponent<IBuilder>() != null)
            {
                behaivourUIEvent?.Invoke(unit as IBuilder);
                return;
            }
        }
        foreach (PlayerUnitBase unit in _selectUnit)
        {
            if (unit.GetComponent<IAttacker>() != null)
            {
                behaivourUIEvent?.Invoke(unit as IAttacker);
                return;
            }
        }
    }
    
    private void UnitAClickAttackAndMoveEvent(Define.MouseEventType type)
    {
        if (_selectUnit.Count == 0)
            return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int mask =  (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
        bool raycastHit = Physics.Raycast(ray, out hit, float.MaxValue, mask);
        if (raycastHit && type == Define.MouseEventType.LeftClick)
        {
            if (_myState == State.ClickA)
            {
                if (hit.collider.gameObject.layer == (int)Define.Layer.Ground)
                {
                    List<Vector3> targetPositionList =
                        GetPositionListAround(hit.point, new float[] { 3f, 6f, 9f }, new int[] { 5, 10, 20 });
                    int targetPositionListIndex = 0;
                    foreach (PlayerUnitBase player in _selectUnit)
                    {
                        player.PlayerUnitMove(targetPositionList[targetPositionListIndex], Define.State.Patrol);
                        targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
                    }
                }
                else if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                {
                    foreach (PlayerUnitBase player in _selectUnit)
                    {
                        player.PlayerUnitAttack(Define.State.Move, hit.collider.gameObject);
                    }
                }
            }
            else if (_myState == State.ClickQ)
            {
                if (hit.collider.gameObject.layer == (int)Define.Layer.Ground)
                {
                    List<Vector3> targetPositionList =
                        GetPositionListAround(hit.point, new float[] { 3f, 6f, 9f }, new int[] { 5, 10, 20 });
                    int targetPositionListIndex = 0;
                    foreach (PlayerUnitBase player in _selectUnit)
                    {
                        player.PlayerUnitMove(targetPositionList[targetPositionListIndex], Define.State.Move);
                        targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
                    }
                }
            }
        }
        _myState = State.None;
    }

    public void SetAllUnitState(Define.State state)
    {
        if (_selectUnit.Count == 0)
            return;

        foreach (PlayerUnitBase player in _selectUnit)
        {
            player.MyState = state;
        }
    }
}
