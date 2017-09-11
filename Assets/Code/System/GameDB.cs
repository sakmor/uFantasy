using System.Collections.Generic;
using UnityEngine;
public class GameDB
{
    public Dictionary<string, string[]> biologyDB;
    public Dictionary<string, string[]> biologyDraw;
    public Dictionary<string, string[]> biologyModel;
    private JsonParse JsonParse;

    private static GameDB _Instance;

    private GameDB()
    {
        JsonParse = new JsonParse();
        LoadDB();
    }

    public static GameDB Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = new GameDB();
            return _Instance;
        }
    }

    public void LoadDB()
    {
        biologyDB = JsonParse.LoadBiologyDB();
        biologyDraw = JsonParse.LoadBiologyDraw();
        biologyModel = JsonParse.LoadBiologyModel();
    }
}
