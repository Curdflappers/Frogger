using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class SaveLoad
{
    const string ADDRESS = "/statistics.curd";

    private static string path = Application.persistentDataPath + ADDRESS;

    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);
        bf.Serialize(file, FrogBehavior.highScore);
        file.Close();
    }

    public static void Load()
    {
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            FrogBehavior.highScore = (int)bf.Deserialize(file);
            file.Close();
        }
    }
}