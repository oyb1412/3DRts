using UnityEngine;
using UnityEngine.UI;

public class UIDragImage : UIBase
{
    private new enum Images
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
        Managers.Input.OnMouseEvent += OnDragEvent;
        Bind<Image>( typeof(Images));
        _dragImage = Get<Image>((int)Images.Drag).GetComponent<Image>();
    }

    private void OnDragEvent(Define.MouseEventType type)
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
        var units = GameObject.FindObjectsByType<PlayerUnitBase>(FindObjectsSortMode.None);
        foreach (var t in units)
        {
            if (_bound.Contains(Camera.main.WorldToScreenPoint(t.transform.position)))
            {
                Managers.Instance.UnitController.SelectedUnit(t.gameObject);
            }
        }
    }
}
