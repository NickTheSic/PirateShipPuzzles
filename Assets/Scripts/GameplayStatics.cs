using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameplayStatics 
{
    //This will keep all of the level checks consistent and easy to change
    public static BlockType GetTypeFromLevel(LevelData ld, Vector3Int loc)
    {
        int type = ld.Level[loc.z].Level[loc.x];

        if (type > -1 && type < (int)BlockType.MaxTypes)
        {
            return (BlockType)type;
        }

        //Not sure if Asserts are editor only or not
#if UNITY_EDITOR    
        UnityEngine.Assertions.Assert.IsTrue(false);
#endif
        //This shouldn't happen in normal circumstances
        return BlockType.Error;

    }


}
