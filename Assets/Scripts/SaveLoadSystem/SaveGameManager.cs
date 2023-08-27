using UnityEngine;
using System.IO;
using SaveLoadSystem;
using UnityEngine.Events;


public class SaveGameManager : MonoBehaviour
{
    public static SaveData data;

    private void Awake()
    {
        data = new SaveData();
        SaveLoad.OnLoadGame += LoadData;
    }
    
    public void DeleteData()
    {
        SaveLoad.DeleteSaveData();
    }
    
    public static void SaveData()
    {
        var saveData = data;
        SaveLoad.SaveGame(saveData);
    }
    
    public static void LoadData(SaveData _data)
    {
        data = _data;
    }
    
    public static void TryLoadData()
    {
        data = SaveLoad.LoadGame();
    }
}