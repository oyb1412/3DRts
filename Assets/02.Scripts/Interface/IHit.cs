using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHit
{
    public void IHit(Status status);
    public Action<float> OnHpEvent { get; set; }
}
