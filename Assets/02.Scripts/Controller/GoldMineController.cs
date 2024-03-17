using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoldMineController : MonoBehaviour, IInstallation
{
    public GameObject GetThisObject()
    {
        return gameObject;
    }
}
