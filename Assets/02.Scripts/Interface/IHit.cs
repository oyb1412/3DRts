using System;

public interface IHit
{
    public void Hit(IAttackerStatus status);
    public Action<float> OnHpEvent { get; set; }
    public Action DeleteHpBarEvent { get; set; }
}
