using System.Collections.Generic;
using UnityEngine;
public class GameDB
{
    public Dictionary<string, string[]> biologyDB;
    public Dictionary<string, string[]> biologyDraw;
    public Dictionary<string, string[]> biologyModel;
    private JsonParse JsonParse;

    private static readonly GameDB _instance = new GameDB();
    public static GameDB Instance { get { return _instance; } }

    private GameDB()
    {
        JsonParse = new JsonParse();
        LoadDB();
    }

    public void LoadDB()
    {
        biologyDB = JsonParse.LoadBiologyDB();
        biologyDraw = JsonParse.LoadBiologyDraw();
        biologyModel = JsonParse.LoadBiologyModel();
    }
}
