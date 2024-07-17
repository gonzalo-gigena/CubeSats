using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Position
{
    public List<double> sun_pos { get; set; }
    public List<double> sc_pos_i { get; set; }
    public double jd { get; set; }
    public List<double> subsolar_point { get; set; }
}

public class PositionLoader
{
    private string jsonFilePath = "generated_positions";

    public List<Position> LoadData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFilePath);
        string jsonContent = jsonFile.text;
        List<Position> positions = JsonConvert.DeserializeObject<List<Position>>(jsonContent);

        return positions;
    }

}