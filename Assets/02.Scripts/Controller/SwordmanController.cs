public class SwordManController : PlayerUnitBase,  IAttacker
{
    protected override void Start()
    {
        base.Start();
        MyType = Type.SwordMan;
        UIBehavior = new AttackerUIBehavior();
    }
    
    protected override void OnAttackEvent()
    {
        if (LockTarget == null)
            return;
        
        LockTarget.GetComponent<IHit>().Hit(Status);
    }
}
