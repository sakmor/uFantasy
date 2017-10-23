using System.Collections.Generic;
using UnityEngine;
public class GameDB
{
    readonly public Dictionary<string, string[]> biologyDB;
    readonly public Dictionary<string, string[]> biologyDraw;
    readonly public Dictionary<string, string[]> biologyModel;
    readonly public Dictionary<string, string[]> biologyAi;
    private JsonParse JsonParse;

    private static readonly GameDB _instance = new GameDB();
    public static GameDB Instance { get { return _instance; } }

    private GameDB()
    {
        JsonParse = new JsonParse();
        biologyDB = JsonParse.LoadBiologyDB();
        biologyDraw = JsonParse.LoadBiologyDraw();
        biologyModel = JsonParse.LoadBiologyModel();
        biologyAi = JsonParse.LoadBiologyAi();
    }
}
