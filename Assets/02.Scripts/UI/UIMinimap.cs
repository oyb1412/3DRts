using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMinimap : UIMap
{
    [SerializeField]private CameraController Camera;
    private enum RawImages
    {
        MinimapRawImage,
    }

    private enum Images
    {
        MinimapIndicator,
    }

    private List<Image> _indicatorList = new List<Image>();
    private RectTransform _indicatorRect;
    protected override void Start()
    {
        base.Start();
        Bind<RawImage>(typeof(RawImages));
        Bind<Image>(typeof(Images));
        for (int i = 0; i < Enum.GetValues(typeof(RawImages)).Length; i++)
        {
            RawImage.Add(Get<RawImage>(i).GetComponent<RawImage>());
        }
        for (int i = 0; i < Enum.GetValues(typeof(Images)).Length; i++)
        {
            _indicatorList.Add(Get<Image>(i).GetComponent<Image>());
        }
        BindEvent(RawImage[(int)RawImages.MinimapRawImage].gameObject,ClickedIndicator,Define.MouseEventType.LeftClick);
        _indicatorRect = _indicatorList[(int)Images.MinimapIndicator].GetComponent<RectTransform>();
        RawImage[(int)RawImages.MinimapRawImage].texture = Texture;
    }

    private void ClickedIndicator(PointerEventData data)
    {
        Vector2 size = _indicatorRect.sizeDelta;
        _indicatorRect.anchoredPosition = data.position - new Vector2(size.x / 4, size.y / 2);
        
        RectTransform miniMapRect = RawImage[(int)RawImages.MinimapRawImage].GetComponent<RectTransform>();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(miniMapRect, data.position, null, out Vector2 localPoint);

        Vector2 miniMapSize = miniMapRect.sizeDelta;
        float xRatio = (localPoint.x + miniMapSize.x / 2) / miniMapSize.x;
        float yRatio = (localPoint.y + miniMapSize.y / 2) / miniMapSize.y;
        
        float mapWidth = 100f; 
        float mapHeight = 100f; 

        Vector3 newPosition = new Vector3(
            xRatio * mapWidth - mapWidth / 32, 
            Camera.transform.position.y,     
            yRatio * mapHeight - mapHeight / 4 
        );

        Camera.transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z);

    }
    private void LateUpdate()
    {
        Vector3 cameraPos = Camera.transform.position - Camera.DefaultPos;

        float xRatio = cameraPos.x  / Managers.Instance.Node.Nodes.GetLength(0);
        float zRatio = cameraPos.z / Managers.Instance.Node.Nodes.GetLength(1);
        float indicatorPosX = xRatio * GetComponentInParent<RectTransform>().anchoredPosition.x * (1080f / 1920f);
        float indicatorPosZ = zRatio * GetComponentInParent<RectTransform>().anchoredPosition.y;
        _indicatorRect.anchoredPosition =
          new Vector2(indicatorPosX, indicatorPosZ);
    }
}
