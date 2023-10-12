using System.IO;
using UnityEditor;
using UnityEngine;

public class MachineCreator : EditorWindow
{
    private string machineName = "";
    private string chosenDirectory = "";
    private string[] directories;

    [MenuItem("Assets/Create/StateMachine/New Machine")]
    private static void ShowWindow()
    {
        MachineCreator window = GetWindow<MachineCreator>("New Machine");
        window.minSize = new Vector2(250, 120);
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Enter Machine Name:", EditorStyles.boldLabel);

        machineName = EditorGUILayout.TextField("Machine Name", machineName);

        GUILayout.Space(10);

        if (directories != null && directories.Length > 0)
        {
            int selectedIndex = EditorGUILayout.Popup("Directory", GetSelectedIndex(), directories);
            chosenDirectory = directories[selectedIndex];
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Create"))
        {
            CreateNewMachine();
            Close();
        }
    }
    
    
    private void CreateNewMachine()
    {
        if (string.IsNullOrEmpty(machineName))
        {
            Debug.LogError("Please enter a machine name.");
            return;
        }

        chosenDirectory = machineName + " States";

        // Create the new directory "Machines"
        string machinesDirectoryPath = Path.Combine("Assets", "InternalAssets", "Scripts", "State Machine", "Machines");
        Directory.CreateDirectory(machinesDirectoryPath);

        // Create the new directory for the states
        string statesDirectoryPath = Path.Combine("Assets", "InternalAssets", "Scripts", "State Machine", "States", chosenDirectory);
        Directory.CreateDirectory(statesDirectoryPath);

        CreateMachineScript(machineName, machinesDirectoryPath);
    }
    
    private void CreateMachineScript(string machineName, string statesDirectoryPath)
    {
        string scriptPath = Path.Combine(statesDirectoryPath, machineName + ".cs");
        if (!File.Exists(scriptPath))
        {
            string scriptContent = @"
using UnityEngine;
using StateManager;

    public class " + machineName + @": MonoBehaviour
    {
        private StateMachine _SM;

        private void InitStates()
        {
            _SM = new StateMachine();
        }
        
        private void Awake()
        {
            InitStates();
        }

        private void Update()
        {
            _SM.CurrentState.Update();
        }
    }";
            File.WriteAllText(scriptPath, scriptContent);
        }
        AssetDatabase.Refresh();
    }
    
    
    private int GetSelectedIndex()
    {
        if (string.IsNullOrEmpty(chosenDirectory) || directories == null)
            return 0;

        for (int i = 0; i < directories.Length; i++)
        {
            if (directories[i] == chosenDirectory)
                return i;
        }

        return 0;
    }
}
