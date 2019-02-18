﻿using System.IO;
using UnityEngine;

public struct SaveProgressObject
{
    public float LevelsUnlocked;
    public float CreaturesUnlocked;
}


public struct SaveProgressSystem : ISaveSystem
{

    private static readonly string SAVE_FOLDER =@"C:\Users\lab\Documents\Games\LostBotanist" + "/SaveData/";

    public void Save()
    {
        if (!Directory.Exists(SAVE_FOLDER + "/Progress"))
        {
            Directory.CreateDirectory(SAVE_FOLDER + "/Progress");
        }

        var currentSaveNumber = 0;

        while (File.Exists(SAVE_FOLDER + "/Progress" + "/progress" + currentSaveNumber + ".json"))
        {
            currentSaveNumber++;
        }

        var saveObject = new SaveProgressObject()
        {
            LevelsUnlocked = 13,
            CreaturesUnlocked = 31
        };

        var json = JsonUtility.ToJson(saveObject);

        File.WriteAllText(SAVE_FOLDER + "/Progress" + "/progress" + currentSaveNumber + ".json", json);
    }


    public object Load()
    {
        var dirInfo = new DirectoryInfo(SAVE_FOLDER + "/Progress");

        var files = dirInfo.GetFiles("*.json");

        FileInfo mostRecent = null;

        foreach (var fileInfo in files)
        {
            if (mostRecent == null)
            {
                mostRecent = fileInfo;
            }
            else
            {
                if (fileInfo.LastWriteTime > mostRecent.LastWriteTime)
                    mostRecent = fileInfo;
            }
        }

        if (mostRecent == null) return null;
        
        var json = File.ReadAllText(mostRecent.FullName);            
        var saveObject = JsonUtility.FromJson<SaveProgressObject>(json);
        Debug.Log(saveObject.LevelsUnlocked);
        Debug.Log(saveObject.CreaturesUnlocked);
        return saveObject;
    }
}