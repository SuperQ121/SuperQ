using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using Application = UnityEngine.Application;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string fileName;

    private GameData gameData;
    private List<ISaveManager> saveManagers;
    private FileDataHandler dataHandler;

    [ContextMenu("delet save file")]
    private void DeleteSaveData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        dataHandler.Delete();
    }


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
            instance = this;
    }

    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath,fileName);
        //Application.persistentDataPath
        //这是一个Unity API中的属性，它返回一个路径字符串，指向一个持久数据目录，这个目录可以用于存储需要跨会话保持的数据。
        //这个路径在不同的操作系统和平台上可能不同，但Unity确保这个目录可以被应用程序读写。
        saveManagers = FindAllSavemanagers();

        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData =dataHandler.Load();

        if (this.gameData == null)
        {
            Debug.Log("No saved data found!");
            NewGame();
        }

        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }

    }

    public void SaveGame()
    {
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(gameData);
        }

        dataHandler.Save(gameData);

    }

    private void OnApplicationQuit()
    {
        //SaveGame();
    }

    private List<ISaveManager> FindAllSavemanagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();

        return new List<ISaveManager>(saveManagers);
    }
}
