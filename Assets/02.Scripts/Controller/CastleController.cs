using System;
using System.Collections.Generic;
using UnityEngine;

public class CastleController : CreatorBuildingBase
{
    protected override void Start()
    {
        base.Start();
        UIBehavior = new CastleUIBehavior();
        
    }

    public override void Init()
    {
        GameObject go = Resources.Load<GameObject>("Prefabs/Unit/Worker");
        Units.Add(go);
    }

    public override void SetCreating(int index)
    {
        //건물 건설중일땐 리턴
        if (BuildState is Building.State.BuildState)
            return;

        //현재 생성중인 유닛이 동시에 생성가능한 최대 유닛보다 많으면 리턴
        if (CurrentCreateNumber >= MaxCreateNumber)
        {
            Debug.Log("생성가능 유닛 자리 없음");
            return;
        }

        CurrentCreateNumber++;

        //이미 state가 create면 그 state를 사용
        if (BuildState is Building.State.CreateState build)
        {
            OnCreateStartEvent?.Invoke();
            build.SetQueue(index);
        }
        //첫 생성이면 state새롭게 생성
        else
        {
            OnCreateStartEvent?.Invoke();
            BuildState = new Building.State.CreateState(index);
        }
        

    }

}
