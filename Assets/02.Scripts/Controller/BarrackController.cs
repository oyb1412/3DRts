using System;

public class BarrackController : BuildingBase
{
    protected override void Start()
    {
        UIBehavior = new BarrackUIBehavior();
    }
}
