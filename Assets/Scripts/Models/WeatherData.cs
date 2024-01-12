
using System.Collections.Generic;

[System.Serializable]
public class WeatherData
{
    public Coord coord;
    public List<Weather> weather;
    public string baseStr;
    public Main main;
    public int visibility;
    public Wind wind;
    public Clouds clouds;
    public int dt;
    public Sys sys;
    public int timezone;
    public int id;
    public string name;
    public int cod;
}