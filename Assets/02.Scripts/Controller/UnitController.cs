using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    private List<PlayerController> _selectUnit = new List<PlayerController>();
    private PlayerController _lastSelectUnit;
    private MonsterController _lastSelectMonster;
    [SerializeField]private bool _isAClickTrigger;
    private void Start()
    {
        Managers.Input.OnMouseEvent += UnitSelectEvent; 
        Managers.Input.OnMouseEvent += UnitAttackAndMoveEvent;
        Managers.Input.OnMouseEvent += UnitAClickAttackAndMoveEvent;
        Managers.Input.OnKeyboardEvent += OnAClickTrigger;
    }

    public void UnitSelectEvent(Define.MouseEventType type)
    {
        if (_isAClickTrigger)
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
                    if (hit.collider.GetComponent<PlayerController>())
                    {
                        if (_lastSelectMonster != null)
                        {
                            _lastSelectMonster.MySelect = BaseController.Select.Deselect;
                            _lastSelectMonster = null;
                        }
                        //마지막으로 선택한 유닛이 존재하고
                        //마지막으로 선택한 유닛과 새롭게 선택한 유닛의 종류가 같다면,
                        if (_lastSelectUnit != null && hit.collider.GetComponent<PlayerController>() == _lastSelectUnit)
                        {
                            //일정 범위내의 그 종류의 유닛을 모두 선택한다.
                            PlayerController[] players = Util.SelectedAutoUnits(_lastSelectUnit, _lastSelectUnit.MyType, 30f, 10);
                            SelectedUnits(players);
                            _lastSelectUnit = null;
                            return;
                        }
                        else
                        {
                            //마지막으로 선택한 유닛을 저장한다.
                            _lastSelectUnit = hit.collider.GetComponent<PlayerController>();
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
                    else if (hit.collider.GetComponent<MonsterController>())
                    {
                        DeSelectedUnit();
                        _lastSelectMonster = hit.collider.GetComponent<MonsterController>();
                        _lastSelectMonster.MySelect = BaseController.Select.Select;
                    }
                    
                }
                else
                {
                    if (_lastSelectMonster != null)
                    {
                        _lastSelectMonster.MySelect = BaseController.Select.Deselect;
                        _lastSelectMonster = null;
                    }

                    DeSelectedUnit();
                }
            }
                break;
        }
    }

    private void DeSelectedUnit()
    {
        foreach (PlayerController player in _selectUnit)
        {
            player.MySelect = PlayerController.Select.Deselect;
        }
        _selectUnit.Clear();
    }

    private void SelectedUnit(RaycastHit hit)
    {
        PlayerController player = hit.collider.GetComponent<PlayerController>();
        player.MySelect = PlayerController.Select.Select;
        _selectUnit.Add(player);
    }
    
    public void SelectedUnit(GameObject go)
    {
        PlayerController player = go.GetComponent<PlayerController>();
        player.MySelect = PlayerController.Select.Select;
        _selectUnit.Add(player);
    }
    
    private void SelectedUnits(PlayerController[] player)
    {
        if (player.Length <= 0)
            return;
        
        for (int i = 0; i < player.Length; i++)
        {
            if (player[i] == null)
                return;
                
            player[i].MySelect = PlayerController.Select.Select;
            _selectUnit.Add(player[i]);
        }
    }
    
    private void UnitAttackAndMoveEvent(Define.MouseEventType type)
    {       
        if (_selectUnit.Count == 0 || _isAClickTrigger)
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
                foreach (PlayerController player in _selectUnit)
                {
                    player.DestPos = hit.point;
                    player.LockTarget = null;
                    player.MyState = PlayerController.State.Move;
                }
                
            }
            else if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {
                foreach (PlayerController player in _selectUnit)
                {
                    player.MyState = PlayerController.State.Move;
                    player.LockTarget = hit.collider.gameObject;
                }
                
            }
        }
    }

    private void OnAClickTrigger()
    {
        if (_selectUnit.Count == 0)
            return;
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (!_isAClickTrigger)
                _isAClickTrigger = true;
            else if (_isAClickTrigger)
                _isAClickTrigger = false;
        }
    }

    private void UnitAClickAttackAndMoveEvent(Define.MouseEventType type)
    {
        if (!_isAClickTrigger || _selectUnit.Count == 0)
            return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position,ray.direction * 100f,Color.red,1f);
        RaycastHit hit;
        int mask =  (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
        bool raycastHit = Physics.Raycast(ray, out hit, float.MaxValue, mask);
        if (raycastHit && type == Define.MouseEventType.LeftClick)
        {
            if(hit.collider.gameObject.layer == (int)Define.Layer.Ground)
            {
                foreach (PlayerController player in _selectUnit)
                {
                    player.DestPos = hit.point;
                    player.LockTarget = null;
                    player.MyState = PlayerController.State.Patrol;
                }
                
            }
            else if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {
                foreach (PlayerController player in _selectUnit)
                {
                    player.MyState = PlayerController.State.Move;
                    player.LockTarget = hit.collider.gameObject;
                }
                
            }
        }
        
        _isAClickTrigger = false;
    }
}
