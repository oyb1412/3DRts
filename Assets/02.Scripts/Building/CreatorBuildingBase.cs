using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorBuildingBase : BuildingBase
{
    public Action<float> OnCreateSliderEvent;
    public Action<int> OnCreateImageEvent;
    public Action OnCreateCompleteEvent;
    public Action OnCreateStartEvent;
    public List<GameObject> Units { get; set; } = new List<GameObject>();
    public MeshRenderer Mesh { get; set; }
    public Define.UnitCreatePos[,] Bound { get; set; }
    public float CurrentCreateTime { get; set; }
    public float MaxCreateTime { get; set; }
    public int CurrentCreateNumber { get; set; }
    public int MaxCreateNumber { get; set; }
    
    public override void SetSelectedState(Define.Select selectedState)
    {
        DefaultSelectedState(selectedState);
        switch (selectedState)
        {
            case Define.Select.Deselect:
            {
                OnBuildEvent = null;
                OnHpEvent = null;
                OnCreateSliderEvent = null;
                OnCreateImageEvent = null;
            }
                break;
            case Define.Select.Select:
                break;
        }
    }

    protected override void Start()
    {
        base.Start();
        CurrentCreateNumber = 0;
        MaxCreateNumber = 5;
        MaxCreateTime = 10f;
        CurrentCreateTime = 0;
    }

    public override void Init()
    {
        Mesh = GetComponent<MeshRenderer>();
        Bound = Util.SetBuildingUnitCreatePos(Mesh, transform);
    }

    public virtual void SetCreating(int index){}

}
