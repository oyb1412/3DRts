using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector2Int _rimitMin;
    private Vector2Int _rimitMax;
    [SerializeField] private int _rimit;
    [SerializeField] private float _speed;
    private void Start()
    {
        _rimitMin.x = Screen.width - Screen.width + _rimit;
        _rimitMin.y = Screen.height - Screen.height + _rimit;
        
        _rimitMax.x = Screen.width - _rimit;
        _rimitMax.y = Screen.height - _rimit;
    }

    private void LateUpdate()
    {
        //SetPosition();
    }

    private void SetPosition()
    {
        if (Input.mousePosition.x < _rimitMin.x)
        {
            transform.position += Vector3.left * (_speed * Time.deltaTime);
        }
        else if(Input.mousePosition.x > _rimitMax.x)
        {
            transform.position -= Vector3.left * (_speed * Time.deltaTime);
        }

        if (Input.mousePosition.y < _rimitMin.y)
        {
            transform.position += Vector3.back * (_speed * Time.deltaTime);
        }
        else if (Input.mousePosition.y > _rimitMax.y)
        {
            transform.position -= Vector3.back * (_speed * Time.deltaTime);
        }
    }
}
