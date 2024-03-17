using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Define.SceneType SceneType { get; protected set; } = Define.SceneType.None;

    public abstract void Clear();

    protected virtual void Init()
    {
        var obj = GameObject.FindFirstObjectByType(typeof(EventSystem));
        if (obj == null)
            Managers.Resources.Activation("UI/EventSystem", null).name = "@EventSystem";
    }

    private void Start()
    {
        Init();
    }
}
