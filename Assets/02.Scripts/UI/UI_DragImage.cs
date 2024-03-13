using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_DragImage : UIBase
{
    public enum Images
    {
        Drag,
    }

    [SerializeField]private Vector2 _startPos;
    [SerializeField]private Vector2 _endPos;
    private RectTransform _dragRectangle;
    private Image _dragImage;

    [SerializeField]private Rect _bound;
    
    private void Start()
    {
        Managers.Input.OnMouseEvent += OnDragEvent;
        Bind<Image>( typeof(Images));
        _dragImage = GetImage((int)Images.Drag);
    }

    public void OnDragEvent(Define.MouseEventType type)
    {
        if (type == Define.MouseEventType.LeftClick)
        {
            _startPos = Input.mousePosition;
        }

        if (type == Define.MouseEventType.Press)
        {
            _endPos = Input.mousePosition;
            DrawDragRectangle();
        }

        if (type == Define.MouseEventType.PressUp)
        {
            SetBound();
            SelectedRectangleUnits();
            _startPos = _endPos = Vector2.zero;
            DrawDragRectangle();
        }
    }

    private void DrawDragRectangle()
    {
        _dragImage.rectTransform.position = (_startPos + _endPos) * .5f;
        _dragImage.rectTransform.sizeDelta = new Vector2(Mathf.Abs(_startPos.x - _endPos.x), Mathf.Abs(_startPos.y - _endPos.y));
    }

    private void SetBound()
    {
        //각 바운드의 4개 점의 위치를 찾기
        if (Input.mousePosition.x < _startPos.x)
        {
            _bound.xMin = Input.mousePosition.x;
            _bound.xMax = _startPos.x;
        }
        else
        {
            _bound.xMax = Input.mousePosition.x;
            _bound.xMin = _startPos.x;
        }

        if (Input.mousePosition.y < _startPos.y)
        {
            _bound.yMin = Input.mousePosition.y;
            _bound.yMax = _startPos.y;
        }
        else
        {
            _bound.yMax = Input.mousePosition.y;
            _bound.yMin = _startPos.y;
        }
    }

    private void SelectedRectangleUnits()
    {
        var units = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < units.Length; i++)
        {
            if (_bound.Contains(Camera.main.WorldToScreenPoint(units[i].transform.position)))
            {
                Managers.Instance.UnitController.SelectedUnit(units[i]);
            }
        }
    }
}
