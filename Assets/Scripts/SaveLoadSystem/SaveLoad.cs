using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace SaveLoadSystem
{
    public class SaveLoad : MonoBehaviour
    {
        public static UnityAction OnSaveGame;
        public static UnityAction<SaveData> OnLoadGame;
        
        private const string DirectoryPath = "/Saves";
        private const string FileName = "/saveData.sav";
        
        public static bool SaveGame(SaveData data)
        {
            OnSaveGame?.Invoke();
            var dir = Application.persistentDataPath + DirectoryPath;
            
            GUIUtility.systemCopyBuffer = dir;
            
            if(!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(dir + FileName, json);
            
            GUIUtility.systemCopyBuffer = dir;

            return true;
        }
        
        public static SaveData LoadGame()
        {
            

            var dir = Application.persistentDataPath + DirectoryPath;
            SaveData data = new SaveData();

            if (File.Exists(dir + FileName))
            {
                string json = File.ReadAllText(dir + FileName);
                data = JsonUtility.FromJson<SaveData>(json);
            
                OnLoadGame?.Invoke(data); 
            }
            else
            {
                Debug.Log("No save file found");
            }


            
            return data;
        }
        
        public static void DeleteSaveData()
        {
            var dir = Application.persistentDataPath + DirectoryPath;
            if (File.Exists(dir + FileName))
            {
                File.Delete(dir + FileName);
            }
        }
    }
}