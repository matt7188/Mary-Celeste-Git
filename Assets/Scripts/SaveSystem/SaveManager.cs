using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public enum Characters {test1, test2};
    public enum Acts {Act1, Act2, Act3};

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            SaveMetrics();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            LoadMetrics();
        }
    }

    public void SaveMetrics()
    {
        Debug.Log("Saved");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream;

        Debug.Log(Application.persistentDataPath + "/" + "SaveState.sav");
        stream = new FileStream(Application.persistentDataPath + "/" + "SaveState.sav", FileMode.Create);

        List<Characters> tempMurderedCrewmen = new List<Characters>();
        tempMurderedCrewmen.Add(Characters.test1);
        Acts tempActiveAct = Acts.Act1;
        List<GameObject> tempInventoryItems = new List<GameObject>();

        SaveData data = new SaveData(tempMurderedCrewmen,tempActiveAct,tempInventoryItems);
        bf.Serialize(stream, data);

        stream.Close();
    }

    public void LoadMetrics()
    {
        if (File.Exists(Application.persistentDataPath + "/" + "SaveState.sav"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/" + "SaveState.sav", FileMode.Open);

            SaveData data = bf.Deserialize(stream) as SaveData;

            stream.Close();

            Debug.Log(data.MurderedCrewmen);
            Debug.Log(data.ActiveAct);
            Debug.Log(data.InventoryItems);
        }
        else
        {
            Debug.LogError("File does not exist.");
        }
    }

    [System.Serializable]
    public class SaveData
    {
        public List<Characters> MurderedCrewmen;
        public Acts ActiveAct;
        public List<GameObject> InventoryItems;

        public SaveData(List<Characters> currentMurderedCrewmen, Acts currentAct, List<GameObject> currentInventoryItems)
        {
            MurderedCrewmen = currentMurderedCrewmen;
            ActiveAct = currentAct;
            InventoryItems = currentInventoryItems;
        }
    }
}
