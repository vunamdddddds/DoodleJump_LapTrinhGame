using UnityEngine;
using System.IO;

public class saveData
{
    public static PlayerContainer playerContainer = new PlayerContainer();

    public static ItemContainer itemContainer = new ItemContainer();

    public static SettingContainer settingContainer= new SettingContainer();


    // Lấy đường dẫn lưu file an toàn
    private static string GetPath()
    {
        return Path.Combine(Application.persistentDataPath, "savegame.json");
    }

    private static string GetPathForItems()
    {
        return Path.Combine(Application.persistentDataPath, "items.json");

    }

    public static string GetPathForSetting()
    {
                return Path.Combine(Application.persistentDataPath, "setting.json");

    }

    public static void Save()
    {
        string path = GetPath();
        string itemsPath = GetPathForItems();
        string settingPath = GetPathForSetting();
        // Serialize object thành chuỗi JSON
        string json = JsonUtility.ToJson(playerContainer, true);
       string itemsJson = JsonUtility.ToJson(itemContainer, true);
       string settingJson = JsonUtility.ToJson(settingContainer, true);
        // Ghi xuống file
        File.WriteAllText(path, json);
        File.WriteAllText(itemsPath, itemsJson);
        File.WriteAllText(settingPath, settingJson);
        Debug.Log("Game Saved to: " + path);
    }

    public static bool Load()
    {
        string path = GetPath();
        string itemsPath = GetPathForItems();
        string settingPath= GetPathForSetting();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            playerContainer = JsonUtility.FromJson<PlayerContainer>(json);
            string itemsJson = File.ReadAllText(itemsPath);
            itemContainer = JsonUtility.FromJson<ItemContainer>(itemsJson);
            string settingJson = File.ReadAllText(settingPath);
            settingContainer= JsonUtility.FromJson<SettingContainer>(settingJson);
            Debug.Log("Game Loaded");
            return true;
        }
        else
        {
            Debug.LogWarning("Save file not found, creating new data.");
            playerContainer = new PlayerContainer();
            itemContainer = new ItemContainer();
            return false;
        }
    }
}