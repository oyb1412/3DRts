using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 _dest;

    [SerializeField] private GameObject _player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void LateUpdate()
    {
        SetPosition();
    }

    private void SetPosition()
    {
        Vector3 pos = _player.transform.position + _dest;
        transform.position = pos;
    }
}
