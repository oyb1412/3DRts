using System;
using UnityEngine;
using UnityEngine.UI;

public class UIDragImage : UIBase
{
    private enum Images
    {
        Drag,
    }

    private Vector2 _startPos;
    private Vector2 _endPos;
    private RectTransform _dragRectangle;
    private Image _dragImage;
    private Rect _bound;
    
    private void Start()
    {
        _dragImage = Util.FindChild(gameObject, "Drag").GetComponent<Image>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            _endPos = Input.mousePosition;
            DrawDragRectangle();
        }

        if (Input.GetMouseButtonUp(0))
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
        var units = FindObjectsByType<PlayerUnitBase>(FindObjectsSortMode.None);
        foreach (var t in units)
        {
            if (_bound.Contains(Camera.main.WorldToScreenPoint(t.transform.position)))
            {
                Managers.Instance.UnitController.SelectedUnit(t.gameObject);
            }
        }
    }
}
