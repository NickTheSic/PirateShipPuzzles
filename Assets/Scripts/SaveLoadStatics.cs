using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class SaveLoadStatics
{

    public static void Save(SaveData dat)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/PirateShipPuzzle.save");
        bf.Serialize(file, dat);
        file.Close();
    }

    public static SaveData Load()
    {
        SaveData dat = new SaveData();
        dat.SavedLevel = 0;
        dat.AudioOn = true;

        if (File.Exists(Application.persistentDataPath + "/PirateShipPuzzle.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/PirateShipPuzzle.save", FileMode.Open);
            dat = (SaveData)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            dat.SavedLevel = 0;
            dat.AudioOn = true;
        }

        return dat;
    }

}
