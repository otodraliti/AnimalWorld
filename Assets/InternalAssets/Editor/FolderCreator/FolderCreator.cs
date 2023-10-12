using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class FolderCreator : EditorWindow
{
    private const string PresetsFolder = "Assets/InternalAssets/Editor/Presets/";
    private TextAsset StateMachine;
    private TextAsset Fabric;
    
    
    private const string InternalAssetsFolder = "InternalAssets";
    private const string ExternalAssetsFolder = "ExternalAssets";
    

    private static readonly string[] DefaultFolders =
    {
        "Animations",
        "Audio",
        "Editor",
        "Materials",
        "Meshes",
        "Prefabs",
        "Scripts",
        "Settings",
        "Textures",
        "UI"
    };

    private static readonly Dictionary<string, string[]> SubFolders = new Dictionary<string, string[]>()
    {
        {"Animations", new string[] {"AnimationClips", "Controllers", "Avatars"}},
        {"Audio", new string[] {"AudioClips", "Mixers"}},
        {"Editor", new string[] {"Patterns", "FolderCreator"}},
        {"Settings", new string[] {"GameSettings", "URP"}},
        {"UI", new string[] {"Assets", "Fonts", "Icon"}}
    };


    [MenuItem("Custom Tools/Create Default Folders")]
    private static void SetUpFolders()
    {
        FolderCreator window = CreateInstance<FolderCreator>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 400, 150);
        window.ShowPopup();
    }

    
    private void OnGUI()
    {
        EditorGUILayout.LabelField("Folder Generator");
        this.Repaint();
        GUILayout.Space(70);
        if (GUILayout.Button("Generate!"))
        {
            CreateFolders();
            MoveExistingFolders();
            
            SetTextAssets();
            
            CreateAdditionalFiles();
            this.Close();
        }
        if (GUILayout.Button("Close!"))
        {
            this.Close();
        }
    }


    #region Create Folders

    private static void CreateFolders()
    {
        CreateInternalAssetsFolders();
        CreateExternalAssetsFolder();
    }

    private static void CreateFolderIfNotExists(string folderPath)
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
    }

    private static void CreateInternalAssetsFolders()
    {
        string internalAssetsPath = Path.Combine("Assets", InternalAssetsFolder);

        foreach (string folder in DefaultFolders)
        {
            string folderPath = Path.Combine(internalAssetsPath, folder);
            CreateFolderIfNotExists(folderPath);
        }

        foreach (KeyValuePair<string, string[]> entry in SubFolders)
        {
            string parentFolder = Path.Combine(internalAssetsPath, entry.Key);

            foreach (string subFolder in entry.Value)
            {
                string subFolderPath = Path.Combine(parentFolder, subFolder);
                CreateFolderIfNotExists(subFolderPath);
            }
        }

        AssetDatabase.Refresh();
    }

    private static void CreateExternalAssetsFolder()
    {
        string externalAssetsPath = Path.Combine("Assets", ExternalAssetsFolder);
        CreateFolderIfNotExists(externalAssetsPath);
        AssetDatabase.Refresh();
    }

    #endregion

    #region Move Existing Folders

    private void MoveExistingFolders()
    {
        MoveSceneFolder();
        MovePresetFolder();
        MoveCreateFoldersScript();
    }

    private void MoveAsset(string sourcePath, string targetPath)
    {
        if (AssetDatabase.ValidateMoveAsset(sourcePath, targetPath).Equals(string.Empty))
        {
            AssetDatabase.MoveAsset(sourcePath, targetPath);
        }
        else
        {
            Debug.LogWarning($"Failed to move asset from {sourcePath} to {targetPath}.");
        }
    }

    private void MoveCreateFoldersScript()
    {
        string scriptPath = Path.Combine("Assets", "FolderCreator.cs");
        string targetPath = Path.Combine("Assets", InternalAssetsFolder, "Editor", "FolderCreator", "FolderCreator.cs");
        MoveAsset(scriptPath, targetPath);
    }

    private void MoveSceneFolder()
    {
        string folderPath = Path.Combine("Assets", "Scenes");
        string targetPath = Path.Combine("Assets", InternalAssetsFolder, "Scenes");
        MoveAsset(folderPath, targetPath);
    }

    private void MovePresetFolder()
    {
        string folderPath = Path.Combine("Assets", "Presets");
        string targetPath = Path.Combine("Assets", InternalAssetsFolder, "Editor", "Presets");
        MoveAsset(folderPath, targetPath);
    }

    #endregion

    #region Create Additional Files

    private void CreateAdditionalFiles()
    {
        CreateReadMe();
        CreateStateMachine();
        CreateFabric();
    }

    private void CreateReadMe()
    {
        string readmePath = Path.Combine("Assets", "ReadMe.md");
        if (!File.Exists(readmePath))
        {
            string readmeContent = @"## Описание проекта

Этот проект разрабатывается для создания [описание проекта].


# Проектная архитектура

## Структура папок


### ExternalAssets

- **ExternalAssets**: Содержит внешние ресурсы и плагины, используемые в проекте.


### InternalAssets

- **Animations**: Содержит анимационные файлы, включая анимационные клипы, контроллеры и аватары.
- **Audio**: Включает аудиофайлы, такие как звуковые эффекты и микшеры.
- **Editor**: Содержит скрипты, предназначенные для использования только в редакторе Unity.
- **Materials**: Включает материалы, используемые для отображения объектов в игре.
- **Meshes**: Содержит 3D-модели и меш-файлы.
- **Prefabs**: Содержит префабы, которые могут быть многократно использованы в сценах.
- **Scenes**: Содержит сцены проекта, включая основные игровые сцены и промежуточные сцены.
- **Scripts**: Содержит все скрипты, используемые в проекте, организованные по различным категориям и функциональности.
- **Settings**: Включает файлы настроек проекта, такие как настройки игры и настройки графики.
- **Textures**: Содержит текстуры, используемые для отображения на поверхностях объектов.
- **UI**: Включает графические ресурсы, связанные с пользовательским интерфейсом, такие как изображения, шрифты и иконки.


## Контакты

- Разработчик: [Имя]

- GitHub: [Ссылка на GitHub репозиторий проекта]";

            File.WriteAllText(readmePath, readmeContent);
        }

        AssetDatabase.Refresh();
    }

    private void CreateStateMachine()
    {
        string scriptPath =
            Path.Combine("Assets", InternalAssetsFolder, "Editor", "Patterns", "StateMachineCreator.cs");
        if (!File.Exists(scriptPath))
        {
            string scriptContent = StateMachine.text;
            File.WriteAllText(scriptPath, scriptContent);
        }

        AssetDatabase.Refresh();
    }

    private void CreateFabric()
    {
        string scriptPath = Path.Combine("Assets", InternalAssetsFolder, "Editor", "Patterns", "FabricBaseCreator.cs");
        if (!File.Exists(scriptPath))
        {
            string scriptContent = Fabric.text;
            File.WriteAllText(scriptPath, scriptContent);
        }

        AssetDatabase.Refresh();
    }


    private void SetTextAssets()
    {
        StateMachine = GetTextAssetFromFolder( "Preset_StateMachineCreator");
        Fabric = GetTextAssetFromFolder( "Preset_FabricBaseCreator");
    }
    private TextAsset GetTextAssetFromFolder(string fileName)
    {
        string filePath = Path.Combine(PresetsFolder, fileName + ".txt");
        return AssetDatabase.LoadAssetAtPath<TextAsset>(filePath);
    }


    #endregion
}