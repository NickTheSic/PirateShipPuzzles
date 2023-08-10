using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//As a short so it takes up less memory
public enum BlockType
{
    Error = -1,

    Start = 0,
    Idle, // 1
        
    Left,  // 2
    Right, // 3
    Up,    // 4
    Down,  // 5

    Win,
    Lose,

    MaxTypes,
}
