using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HpBar : UIBase
{
    private IHit _parent;
    private Transform _original;
    private Transform _hpPos;
    
    public enum Sliders
    {
        HpBar,
    }

    void Start()
    {
        _parent = GetComponentInParent<IHit>();
        _original = transform.root;
        _hpPos = transform.parent;
        Bind<Slider>(typeof(Sliders));
        Slider hpbar = Get<Slider>((int)Sliders.HpBar);
        _parent.OnHpEvent += (hp => { hpbar.value = hp; });
        transform.parent.transform.parent = null;
    }

    private void LateUpdate()
    {
        _hpPos.position = _original.position;
    }

    
}
