using UnityEngine;

public class ControllerDemoSaveFile : MonoBehaviour
{
    public static ControllerDemoSaveFile Instance;

    public DialogsConfig DialogsConfig;

    public int testParam;
    public MainData mainData = new MainData();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
        Debug.Log($"Awake ControllerDemoSaveFile");
    }

    private void Start()
    {
        Debug.Log($"Start ControllerDemoSaveFile");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            testParam++;
        }
    }
}