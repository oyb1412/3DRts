using UnityEngine;

public class WorkerController : PlayerUnitBase,IAttacker, IBuilder
{
    protected override void Start()
    {
        base.Start();
        MyType = Type.Worker;
        UIBehavior = new BuilderUIBehavior();
    }

    public void SetBuildState(GameObject go)
    {
        LockTarget = go;
        SetState(new BuildMoveState(this));
    }

    public void OnBuildEvent()
    {
        if (LockTarget == null)
            SetState(new IdleState(this));
    }
    
    protected override void OnAttackEvent()
    {
        if (LockTarget == null)
            return;
        
        LockTarget.GetComponent<IHit>().Hit(Status);
    }
}
