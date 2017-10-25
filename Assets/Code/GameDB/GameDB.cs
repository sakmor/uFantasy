using System.Collections.Generic;
using UnityEngine;
public class GameDB
{
    readonly public Dictionary<string, string[]> BiologyDB;
    readonly public Dictionary<string, string[]> BiologyDraw;
    readonly public Dictionary<string, string[]> BiologyModel;
    readonly public Dictionary<string, string[]> BiologyAi;
    readonly public Dictionary<string, string[]> Level;
    private JsonParse JsonParse;

    private static readonly GameDB _instance = new GameDB();
    public static GameDB Instance { get { return _instance; } }

    private GameDB()
    {
        JsonParse = new JsonParse();
        BiologyDB = JsonParse.LoadBiologyDB();
        BiologyDraw = JsonParse.LoadBiologyDraw();
        BiologyModel = JsonParse.LoadBiologyModel();
        BiologyAi = JsonParse.LoadBiologyAi();
        Level = JsonParse.LoadLevel();
    }
}
