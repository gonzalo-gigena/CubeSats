using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Positions
{
    public List<string> dates { get; set; }
    public List<List<double>> subsolar_points { get; set; }
    public List<List<double>> sun_pos { get; set; }
    public List<Satellite> satellites { get; set; }
}

public class Satellite
{
    public string name { get; set; }
    public List<List<double>> pos { get; set; }
}

public class PositionLoader
{
    private string jsonFilePath = "generated_positions";

    public Positions LoadData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFilePath);
        string jsonContent = jsonFile.text;
        Positions positions = JsonConvert.DeserializeObject<Positions>(jsonContent);

        return positions;
    }

}