using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HpBar : UIBase
{
    private IHit _parent;
    [SerializeField]private Transform _original;
    public enum Sliders
    {
        HpBar,
    }

    void Start()
    {
        _parent = GetComponentInParent<IHit>();
        _original = transform.root;
        Bind<Slider>(typeof(Sliders));
        Slider hpbar = Get<Slider>((int)Sliders.HpBar);
        _parent.OnHpEvent += (hp => { hpbar.value = hp; });
        GameObject go = GameObject.Find("@HpBar_root");
        if (go == null)
        {
            go = new GameObject("@HpBar_root");
        }
        transform.parent.transform.parent = go.transform;
    }

    private void LateUpdate()
    {
        transform.position= _original.transform.position + Vector3.back;
    }
}
