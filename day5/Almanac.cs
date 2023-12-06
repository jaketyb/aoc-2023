public class Almanac
{
    public Almanac()
    {
        SeedToSoil = new List<MapValue>();
        SoilToFertilizer = new List<MapValue>();
        ;
        FertilizerToWater = new List<MapValue>();
        ;
        WaterToLight = new List<MapValue>();
        ;
        LightToTemperature = new List<MapValue>();
        ;
        TemperatureToHumidity = new List<MapValue>();
        ;
        HumidityToLocation = new List<MapValue>();
        ;
    }

    public List<MapValue> SeedToSoil { get; set; }
    public List<MapValue> SoilToFertilizer { get; set; }
    public List<MapValue> FertilizerToWater { get; set; }

    public List<MapValue> WaterToLight { get; set; }

    public List<MapValue> LightToTemperature { get; set; }

    public List<MapValue> TemperatureToHumidity { get; set; }

    public List<MapValue> HumidityToLocation { get; set; }
}
