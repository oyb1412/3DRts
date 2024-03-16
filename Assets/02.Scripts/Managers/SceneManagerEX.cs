using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class SceneManagerEx
{
    public BaseScene CurrentScene => Object.FindFirstObjectByType(typeof(BaseScene)).GetComponent<BaseScene>();

    public void LoadScene(Define.SceneType type)
    {
        SceneManager.LoadScene(GetSceneName(type));
    }

    private string GetSceneName(Define.SceneType type)
    {
       return Enum.GetName(typeof(Define.SceneType), type);
    }
}
