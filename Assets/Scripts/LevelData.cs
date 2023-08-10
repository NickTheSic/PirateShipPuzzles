using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Create Level")]
public class LevelData : ScriptableObject
{
    public Vector3 NewCameraPos = new Vector3(0, 6, -6);

    public LevelStruct[] Level;
    
    public int Width = 0, Height = 0;
}
