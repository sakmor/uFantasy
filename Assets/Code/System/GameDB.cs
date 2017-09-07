using System.Collections.Generic;
using UnityEngine;
public class GameDB
{
    public Dictionary<int, string[]> biologyDB;
    private JsonParse JsonParse;

    public GameDB()
    {
        JsonParse = new JsonParse();
    }

    public void LoadDB()
    {
        biologyDB = JsonParse.LoadBiologyDB();

        Debug.Log("DB Loading Complete");
    }
}
