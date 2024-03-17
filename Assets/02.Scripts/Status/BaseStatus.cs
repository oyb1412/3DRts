
using UnityEngine;

public abstract class BaseStatus : MonoBehaviour
{
    public abstract int AttackDamage{ get; set; }
    public float Hp { get; set; }
    public float MaxHp { get; set; }
    public int Defense { get; set; }
}
