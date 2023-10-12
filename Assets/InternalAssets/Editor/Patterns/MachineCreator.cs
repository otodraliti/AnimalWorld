using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;


public class StatesCreator : EditorWindow
{
    private string stateName = "";
    private string chosenDirectory = "";
    private string[] directories;

    [MenuItem("Assets/Create/StateMachine/New State")]
    private static void ShowWindow()
    {
        StatesCreator window = GetWindow<StatesCreator>("New State");
        window.minSize = new Vector2(250, 120);
        window.LoadDirectories();
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Enter State Name:", EditorStyles.boldLabel);

        stateName = EditorGUILayout.TextField("State Name", stateName);

        GUILayout.Space(10);

        if (directories != null && directories.Length > 0)
        {
            int selectedIndex = EditorGUILayout.Popup("Directory", GetSelectedIndex(), directories);
            chosenDirectory = directories[selectedIndex];
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Create"))
        {
            CreateNewState();
            Close();
        }
    }

    private void CreateNewState()
    {
        if (string.IsNullOrEmpty(chosenDirectory))
        {
            Debug.LogError("Please select a directory.");
            return;
        }

        CreateState(stateName);
    }

    private void CreateState(string stateName)
    {
        string scriptPath = Path.Combine("Assets", "InternalAssets", "Scripts", "State Machine", "States", chosenDirectory, stateName + ".cs");
        if (!File.Exists(scriptPath))
        {
            string scriptContent = @"using StateManager;

public class " + stateName + @": State
{
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
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

    private void LoadDirectories()
    {
        string directoryPath = Path.Combine("Assets", "InternalAssets", "Scripts", "State Machine", "States");
        directories = Directory.GetDirectories(directoryPath)
            .Select(subdirectory => new DirectoryInfo(subdirectory).Name)
            .ToArray();
    }
}