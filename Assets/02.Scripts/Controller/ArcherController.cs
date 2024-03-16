using UnityEngine;

public class ArcherController : PlayerUnitBase,  IAttacker
{
    private GameObject _firePos;
    protected override void Start()
    {
        base.Start();
        MyType = Type.Archer;
        _firePos = Util.FindChild(gameObject, "FirePos");
        UIBehavior = new AttackerUIBehavior();
    }

    protected override void OnAttackEvent()
    {
        if (LockTarget == null)
            return;

        FireProjectile();
    }

    private void FireProjectile()
    {
        ProjectileController arrow = Managers.Resources.Activation("ArrowParent", null).GetComponent<ProjectileController>();
        arrow.Init(LockTarget, _firePos.transform.position, Status);
    }
}
