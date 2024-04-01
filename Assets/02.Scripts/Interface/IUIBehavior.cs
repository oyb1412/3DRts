using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IUIBehavior
{
    void UpdateUI(List<GameObject> panels);
}
