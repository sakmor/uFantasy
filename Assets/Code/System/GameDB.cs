using System.Collections.Generic;
public class GameDB
{
    public Dictionary<int, string[]> biologyDB;
    public void LoadDB()
    {
        JsonParse JsonParse = new JsonParse();
        biologyDB = JsonParse.LoadBiologyDB();
    }
}
