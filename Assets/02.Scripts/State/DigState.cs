using System;
using System.Collections;

using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DigState : IUnitState
{
    private float _digTimer;
    public void OnUpdate(PlayerUnitBase unit)
    {
        _digTimer += Time.deltaTime;
        if (_digTimer < 5) return;
        Managers.Game.SetGoldEvent(-10);
        _digTimer = 0;
    }

}
