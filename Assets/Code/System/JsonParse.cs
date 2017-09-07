using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
public class JsonParse : MonoBehaviour
{
    string Biology = "Biology";

    // Use this for initialization

    public Dictionary<int, string[]> LoadBiologyDB()
    {
        string[] jsonData = Load_DB_Json(Biology);
        Dictionary<int, string[]> Dictionary = new Dictionary<int, string[]>();
        string[] rowData = new string[jsonData.Length];
        for (var i = 0; i < rowData.Length; i++)
        {
            int index = int.Parse((jsonData[i].Trim()).Split(","[0])[0]);
            Dictionary.Add(index, (jsonData[i].Trim()).Split(","[0]));
        }
        return Dictionary;
    }
    private string[] Load_DB_Json(string FileName)
    {
        //讀取json檔案並轉存成文字格式
        StreamReader file = new StreamReader(Application.dataPath + "/Resources/DB/" + FileName + ".json");
        string loadJson = file.ReadToEnd();
        file.Close();

        //新增一個物件類型為playerState的變數 loadData
        JsonClass loadData = new JsonClass();

        //使用JsonUtillty的FromJson方法將存文字轉成Json
        loadData = JsonUtility.FromJson<JsonClass>(loadJson);

        return loadData.Index;

    }

    private class JsonClass
    {
        public string[] Index;
    }
}
