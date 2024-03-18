using System;
using System.Collections.Generic;
using UnityEngine;

public class BarrackController : BuildingBase, IUnitProducer
{

    public List<GameObject> Units { get; set; } = new List<GameObject>();
    public MeshRenderer Mesh { get; set; }
    public Define.UnitCreatePos[,] Bound { get; set; }
    public float CurrentCreateTime { get; set; }
    public float MaxCreateTime { get; set; }
    public int CurrentCreateNumber { get; set; }
    public int MaxCreateNumber { get; set; }

    protected override void Start()
    {
        base.Start();

        CurrentCreateNumber = 0;
        MaxCreateNumber = 5;
        MaxCreateTime = 10f;
        CurrentCreateTime = 0;
        UIBehavior = new BarrackUIBehavior();
    }

    public override void Init()
    {
        Mesh = GetComponent<MeshRenderer>();
        Bound = Util.SetBuildingUnitCreatePos(Mesh, transform);
        GameObject swordMan = Resources.Load<GameObject>("Prefabs/Unit/Swordsman");
        GameObject archer = Resources.Load<GameObject>("Prefabs/Unit/Archer");
        Units.Add(swordMan);
        Units.Add(archer);
    }

   //todo
   //유닛생성 스테이트로 변경

    public void SetCreating(int index)
    {
        //건물 건설중일땐 리턴
        if (BuildState is BuildingBuildState)
            return;

        //현재 생성중인 유닛이 동시에 생성가능한 최대 유닛보다 많으면 리턴
        if (CurrentCreateNumber >= MaxCreateNumber)
        {
            Debug.Log("생성가능 유닛 자리 없음");
            return;
        }

        CurrentCreateNumber++;
        //이미 state가 create면 그 state를 사용
        if (BuildState is BuildingCreateState build)
        {
            OnCreateStartEvent?.Invoke();
            build.SetQueue(index);
        }
        //첫 생성이면 state새롭게 생성
        else
        {
            OnCreateStartEvent?.Invoke();
            BuildState = new BuildingCreateState(index);
        }
    }

 
}
