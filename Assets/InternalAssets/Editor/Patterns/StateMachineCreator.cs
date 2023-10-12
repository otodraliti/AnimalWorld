using UnityEditor;
using UnityEngine;
using System.IO;

namespace StateMachineCreator
{
    public class StateMachineCreator : EditorWindow
    {
        private const string PresetsFolder = "Assets/InternalAssets/Editor/Presets/";
    
        private TextAsset BaseMachine;
        private TextAsset BaseState;
        private TextAsset MachineCreator;
        private TextAsset StateCreator;
        
        
        [MenuItem("Custom Tools/State Machine/Import State Machine")]
        private static void SetUpFolders()
        {
            StateMachineCreator window = CreateInstance<StateMachineCreator>();
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 400, 150);
            window.ShowPopup();
        }
        
        private void OnGUI()
        {
            EditorGUILayout.LabelField("Import State Machine");
            this.Repaint();
            GUILayout.Space(70);
            if (GUILayout.Button("Import!"))
            {
                string folderPath = Path.Combine("Assets", "InternalAssets", "Scripts", "State Machine");
                CreateFolderIfNotExists(folderPath);
                
                SetTextAssets();

                CreateMachineCreator();
                CreateStateCreator();
                
                CreateStateMachine();
                CreateState();
    
                this.Close();
            }
            if (GUILayout.Button("Close!"))
            {
                this.Close();
            }
        }


        private void SetTextAssets()
        {
            BaseMachine = GetTextAssetFromFolder("Preset_CreateStateMachine");
            BaseState = GetTextAssetFromFolder("Preset_CreateState");
            MachineCreator = GetTextAssetFromFolder("Preset_MachineCreator");
            StateCreator = GetTextAssetFromFolder("Preset_StateCreator");
        }
        private TextAsset GetTextAssetFromFolder(string fileName)
        {
            string filePath = Path.Combine(PresetsFolder, fileName + ".txt");
            return AssetDatabase.LoadAssetAtPath<TextAsset>(filePath);
        }
        
        private void CreateStateMachine()
        {
            string scriptPath = Path.Combine("Assets", "InternalAssets", "Scripts", "State Machine","StateMachine.cs");
            if (!File.Exists(scriptPath))
            {
                string scriptContent = BaseMachine.text;
                File.WriteAllText(scriptPath, scriptContent);
            }
            AssetDatabase.Refresh();
        }
        
    private void CreateState()
    {
        string scriptPath = Path.Combine("Assets","InternalAssets", "Scripts", "State Machine", "State.cs");
        if (!File.Exists(scriptPath))
        {
            string scriptContent = BaseState.text;
             File.WriteAllText(scriptPath, scriptContent);
        }
        AssetDatabase.Refresh();
    }

    private void CreateStateCreator()
    {
        string scriptPath = Path.Combine("Assets", "InternalAssets", "Editor", "Patterns","StateCreator.cs");
        if (!File.Exists(scriptPath))
        {
            string scriptContent = MachineCreator.text;
            File.WriteAllText(scriptPath, scriptContent);
        }
        AssetDatabase.Refresh();
    }

    private void CreateMachineCreator()
    {
        string scriptPath = Path.Combine("Assets", "InternalAssets", "Editor", "Patterns","MachineCreator.cs");
        if (!File.Exists(scriptPath))
        {
            string scriptContent = StateCreator.text;
            File.WriteAllText(scriptPath, scriptContent);
        }
        AssetDatabase.Refresh();
    }
    
        
    private static void CreateFolderIfNotExists(string folderPath)
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
    }
    }
}
