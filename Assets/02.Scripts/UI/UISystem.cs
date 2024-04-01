using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UISystem : UIBase
{
    private enum SystemTexts
    {
        GoldText,
    }
    private TextMeshProUGUI _goldTexts;

    private void Start()
    {
        _goldTexts = Util.FindChild(gameObject, "GoldText").GetComponent<TextMeshProUGUI>();

        _goldTexts.text = Managers.Game.CurrentGold + "g";

        Managers.Game.OnGoldEvent += (gold => _goldTexts.text = gold + "g");
    }
}
