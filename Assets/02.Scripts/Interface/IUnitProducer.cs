using System.Collections.Generic;
using UnityEngine;

public interface IUnitProducer
{
    public List<GameObject> Units { get; set; }
    public MeshRenderer Mesh{ get; set; }
    public Define.UnitCreatePos[,] Bound{ get; set; }

    public float CurrentCreateTime { get; set; }
    public float MaxCreateTime { get; set; }
    public int CurrentCreateNumber { get; set; }
    public int MaxCreateNumber { get; set; }
    public void SetCreating(int index);
}
