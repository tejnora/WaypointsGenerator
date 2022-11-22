using System.Device.Location;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Generator
{
    internal class AirportReader
    {
        //https://www.skyfly.cz/databaze-letist.php?cesky=&zeme=&kraj=&povrch=&radit=&stranka=1
        public static List<NavigationPoint> Parse(string filePath)
        {
            var result = new List<NavigationPoint>();
            var lines = File.ReadAllLines(filePath);
            for (var i = 0; i < lines.Length; i += 7)
            {
                result.Add(new NavigationPoint
                {
                    Name = lines[i + 1],
                    Coordinate = ParseCoordinates(lines[i + 2]),
                    Height = ParseHeight(lines[i + 3]),
                    Frequency = lines[i + 4],
                    Type = NavigationPointType.Airport
                });
            }
            return result;
        }

        public static GeoCoordinate ParseCoordinates(string coordinate)
        {
            //"N49°40'14 E17°17'42"
            const string regex = @"^([N,S]{1})([0-9]+)°([0-9]+)'([0-9]+) ([E,W]{1})([0-9]+)°([0-9]+)'([0-9]+)$";
            var match = Regex.Match(coordinate, regex);
            Debug.Assert(match.Success);
            var latitude = double.Parse(match.Groups[2].Value) + double.Parse(match.Groups[3].Value) / 60.0 +
                           double.Parse(match.Groups[4].Value) / 3600.0;
            var longitude = double.Parse(match.Groups[6].Value) + double.Parse(match.Groups[7].Value) / 60.0 +
                           double.Parse(match.Groups[8].Value) / 3600.0;
            if (match.Groups[1].Value == "S") latitude = -latitude;
            if (match.Groups[5].Value == "W") longitude = -longitude;
            return new GeoCoordinate { Latitude = latitude, Longitude = longitude };
        }

        public static int ParseHeight(string height)
        {
            //252m MSL
            const string regex = @"^([0-9]+)m MSL$";
            var match = Regex.Match(height, regex);
            Debug.Assert(match.Success);
            return int.Parse(match.Groups[1].Value);
        }
    }
}
