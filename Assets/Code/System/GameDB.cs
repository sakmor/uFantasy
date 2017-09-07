using System.Collections.Generic;
using UnityEngine;
public class GameDB
{
    public Dictionary<int, string[]> biologyDB;
    public Dictionary<int, string[]> biologyDraw;
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
    }
}
