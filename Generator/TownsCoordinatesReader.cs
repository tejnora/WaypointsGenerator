using System.Device.Location;
using CsvHelper;
using System.Globalization;

namespace Generator;

static class TownsCoordinatesReader
{
    class TownPoint
    {
        public string Name { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

    public static List<NavigationPoint> Parse(string filePath)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<TownPoint>();
        return records.Select(n => new NavigationPoint
        {
            Name = n.Name,
            Coordinate = new GeoCoordinate(double.Parse(n.Latitude), double.Parse(n.Longitude))
        }).ToList();
    }
}