using System;
using System.Collections.Generic;
using UnityEngine;

public class CastleController : BuildingBase, IUnitProducer
{
    public List<GameObject> Units { get; set; } = new List<GameObject>();

    public MeshRenderer Mesh { get; set; }
    public Define.UnitCreatePos[,] Bound { get; set; }
    
    protected override void Start()
    {
        UIBehavior = new CastleUIBehavior();
        GameObject go = Resources.Load<GameObject>("Prefabs/Unit/Worker");
        Units.Add(go);
        Mesh = GetComponent<MeshRenderer>();
        Bound = Util.SetBuildingUnitCreatePos(Mesh, transform);
    }


    public void SetCreating(int index)
    {
        foreach (Define.UnitCreatePos t in Bound)
        {
            int mask = (1 << (int)Define.Layer.Player) | (1 << (int)Define.Layer.Monster) | (1 << (int)Define.Layer.Building) |  (1 << (int)Define.Layer.Mine);
            var rayCastHit = Physics.OverlapSphere(new Vector3(t.X, 2f, t.Z), 1f, mask);
            if (rayCastHit.Length == 0)
            {
                GameObject unit = Managers.Resources.Activation($"Unit/{Units[index].name}", null);
                unit.transform.position = new Vector3(t.X, 2f, t.Z);
                
                //todo
                //유닛 큐에 넣고 스테이트 변경
                //while로 큐에 유닛이 존재하면 유닛 계속 생성
                //각종 데이터 슬슬 따로 보관
                break;
            }
        }
        Debug.Log("유닛을 생성할 공간이 없습니다.");
    }

}
