using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IMiddleUIBehavior
{
    void UpdateUI(Dictionary<string, GameObject> panels, List<IAllUnit> units);
}
