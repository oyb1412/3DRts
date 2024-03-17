
using UnityEngine;

public class GameScene : BaseScene
{
    public override void Clear()
    {
        
    }

    protected override void Init()
    {
        base.Init();
        GameObject castle = Managers.Resources.Activation("Building/Castle", null);
        castle.transform.position = new Vector3(15f, 2f, 15f);
        Managers.Instance.Node.SetNode(castle);
        GameObject mine = Managers.Resources.Activation("Building/GoldMine", null);
        mine.transform.position = new Vector3(21f, 2f, 7f);
        Managers.Instance.Node.SetNode(mine);
        
        for (int i = 0; i < 3; i++)
        {
            GameObject worker = Managers.Resources.Activation("Unit/Worker", null);
            worker.transform.position = new Vector3(10f + (3 * i), 2f, 8f);
        }
    }

  
}
