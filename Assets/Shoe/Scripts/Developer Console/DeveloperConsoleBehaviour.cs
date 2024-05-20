using TMPro;
using UnityEngine;

public class DeveloperConsoleBehaviour : MonoBehaviour
{
    public static DeveloperConsoleBehaviour Instance;

    [SerializeField] private string prefix = string.Empty;
    [SerializeField] private ConsoleCommand[] commands = new ConsoleCommand[0];

    [Header("UI")]
    [SerializeField] GameObject uiCanvas = null;
    [SerializeField] TMP_InputField inputField = null;

    private float pausedTimeScale;
    private DeveloperConsole developerConsole;

    private DeveloperConsole DeveloperConsole
    {
        get
        {
            if (developerConsole != null) { return developerConsole; }
            return developerConsole = new DeveloperConsole(prefix, commands);
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        SetCommandDatabase();
    }

    private void SetCommandDatabase()
    {
        var _obj = Resources.LoadAll("", typeof(ConsoleCommand));
        commands = new ConsoleCommand[_obj.Length];
        for (int i = 0; i < _obj.Length; i++)
        {
            commands[i] = (ConsoleCommand)_obj[i];
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F8))
            Toggle();
    }

    public void Toggle() 
    {
       //if(!context.action.triggered) { return; }

        if (uiCanvas.activeSelf)
        {
            Time.timeScale = pausedTimeScale;
            uiCanvas.SetActive(false);
        }
        else
        {
            pausedTimeScale = Time.timeScale;
            Time.timeScale = 0;
            uiCanvas.SetActive(true);
            inputField.ActivateInputField();
        }
    }

    public void ProcessCommand(string inputValue)
    {
        Debug.Log("trying to pass command " + inputValue);

        DeveloperConsole.ProcessCommand(inputValue);

        inputField.text = string.Empty;

        Toggle();
    }

    public void DisplayErrorLog()
    {
        string currentText = inputField.text;
        inputField.text = "ERROR PASSING COMMAND " + currentText;
        Debug.Log(inputField.text);
    }
} 