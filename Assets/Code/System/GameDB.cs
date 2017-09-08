using System.Collections.Generic;
using UnityEngine;
public class GameDB
{
    public Dictionary<string, string[]> biologyDB;
    public Dictionary<string, string[]> biologyDraw;
    public Dictionary<string, string[]> biologyModel;
    private JsonParse JsonParse;

    public GameDB()
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
