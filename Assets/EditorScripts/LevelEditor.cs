using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class LevelEditor : EditorWindow
{

    string Filename;
    int Width, Height;
    LevelData Data;

    BlockType[,] DataArray;

    [MenuItem("Window/Level Editor")]
    public static void CreateWindow()
    {
        EditorWindow.GetWindow(typeof(LevelEditor));
    }

    private void OnGUI()
    {
        Filename = EditorGUILayout.TextField(Filename);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Create New Asset"))
        {
            CreateData();
        }
        if (GUILayout.Button("Save Asset"))
        {
            SaveData();
        }
        if (GUILayout.Button("Clear"))
        {
            Data = null;
        }

        Width = EditorGUILayout.IntField("Width", Width);
        Height = EditorGUILayout.IntField("Height", Height);

        GUILayout.EndHorizontal();

        if (Data != null)
        {
            for (int r = Width - 1; r >= 0; r--)
            {
                GUILayout.BeginHorizontal();
                for (int c = 0; c < Height; c++)
                {
                    DataArray[r,c] = (BlockType)EditorGUILayout.EnumPopup(DataArray[r, c]);// IntField(Data.Level[r].Level[c]);
                    Data.Level[r].Level[c] = (int)DataArray[r, c];
                }
                GUILayout.EndHorizontal();
            }
            EditorUtility.SetDirty(Data);
        }
    }

    public void CreateData()
    {
        Data = ScriptableObject.CreateInstance<LevelData>();

        Data.Width = Width;
        Data.Height = Height;

        Data.Level = new LevelStruct[Width];

        DataArray = new BlockType[Width, Height];

        for (int i = 0; i < Width; i++)
        {
            Data.Level[i] = new LevelStruct();
            Data.Level[i].Level = new int[Height];
            for (int j = 0; j < Height; j++)
            {
                Data.Level[i].Level[j] = 0;
                DataArray[i, j] = BlockType.Error;
            }
        }

        AssetDatabase.CreateAsset(Data, "Assets/Scripts/LevelS/" + Filename + ".asset");
    }

    public void SaveData()
    {
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = Data;
    }

}

#endif
