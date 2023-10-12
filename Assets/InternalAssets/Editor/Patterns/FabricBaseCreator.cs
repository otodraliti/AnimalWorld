using System.IO;
using UnityEditor;
using UnityEngine;

public class FabricBaseCreator : EditorWindow
{
    private const string PresetsFolder = "Assets/InternalAssets/Editor/Presets/";
    private TextAsset AbstractFabricCreator;
    
    
    [MenuItem("Custom Tools/Fabric/Import Fabric")]
    private static void SetUpFolders()
    {
        FabricBaseCreator window = CreateInstance<FabricBaseCreator>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 400, 150);
        window.ShowPopup();
    }
    
    private void OnGUI()
    {
        EditorGUILayout.LabelField("Fabric Generator");
        this.Repaint();
        GUILayout.Space(70);
        if (GUILayout.Button("Generate!"))
        {
            string folderPath = Path.Combine("Assets", "InternalAssets", "Scripts", "Fabrics");
            CreateFolderIfNotExists(folderPath);
            SetTextAssets();
            
            CreateAbstractFabricCreator();
            
            this.Close();
        }
        if (GUILayout.Button("Close!"))
        {
            this.Close();
        }
    }
    
    private void SetTextAssets()
    {
        AbstractFabricCreator = GetTextAssetFromFolder("Preset_AbstractFabricCreator");
    }
    private TextAsset GetTextAssetFromFolder(string fileName)
    {
        string filePath = Path.Combine(PresetsFolder, fileName + ".txt");
        return AssetDatabase.LoadAssetAtPath<TextAsset>(filePath);
    }
    
    private void CreateAbstractFabricCreator()
    {
        string scriptPath = Path.Combine("Assets", "InternalAssets", "Editor", "Patterns","AbstractFabricCreator.cs");
        if (!File.Exists(scriptPath))
        {
            string scriptContent = AbstractFabricCreator.text;
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
