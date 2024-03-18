using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers _instance;
    public static Managers Instance => _instance;

    private CursorManager _cursor = new CursorManager();
    private GameManager _game = new GameManager();
    private BuildManager _build = new BuildManager();
    private PoolManager _pool = new PoolManager();
    private InputManager _input = new InputManager();
    private ResourcesManager _resources = new ResourcesManager();
    private SceneManagerEx _scene = new SceneManagerEx();
    public UnitController UnitController;
    public Node Node;
    public UIBehaviourPanelController UIBehaviourPanel;
    public UIMiddleStateController UIMiddleStatePanel;
    public static CursorManager Cursor => _instance._cursor;
    public static PoolManager Pool => _instance._pool;
    public static SceneManagerEx Scene => _instance._scene;
    public static InputManager Input => Instance._input;
    public static ResourcesManager Resources => Instance._resources;
    public static BuildManager Build => Instance._build;
    public static GameManager Game => Instance._game;
    
    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        Input.OnUpdate();
        Build.OnUpdate();
    }

    private void Init()
    {
        if (_instance == null)
        {
            GameObject managers = GameObject.Find("@Managers");
            if (managers == null)
            {
                managers = new GameObject("@Managers");
                managers.AddComponent<Managers>();
            }
            
            DontDestroyOnLoad(managers);
            _instance = managers.GetComponent<Managers>();
            
            Game.Init();
            Pool.Init();
            Cursor.Init();
            Build.Init();
        }
    }
    
}
