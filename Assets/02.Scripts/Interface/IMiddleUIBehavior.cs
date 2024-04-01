using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IMiddleUIBehavior
{
    void UpdateUI(List<GameObject> panels, List<IAllUnit> units);
}
