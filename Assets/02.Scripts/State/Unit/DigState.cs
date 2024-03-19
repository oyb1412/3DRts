using System;
using System.Collections;

using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Unit.State
{
    public class DigState : IUnitState
    {
        private float _digTimer;

        public DigState(PlayerUnitBase unit)
        {
            unit.Anime.CrossFade("Dig", .2f);
        }

        public void OnUpdate(PlayerUnitBase unit)
        {
            _digTimer += Time.deltaTime;
            if (_digTimer < 5) return;
            Managers.Game.SetGoldEvent(-10);
            _digTimer = 0;
        }

    }
}
