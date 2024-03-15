using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHit
{
    public void Hit(Status status);
    public Action<float> OnHpEvent { get; set; }
    public Action DeleteHpBarEvent { get; set; }
}
