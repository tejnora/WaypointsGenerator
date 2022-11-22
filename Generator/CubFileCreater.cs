using System.Device.Location;
using System.Text;

namespace Generator
{
    class CubFileCreater
    {
        //https://downloads.naviter.com/docs/CUB_File_Format_v1.pdf
        readonly IList<NavigationPoint> _navigationPoints;

        public CubFileCreater(IList<NavigationPoint> navigationPoints)
        {
            _navigationPoints = navigationPoints;
        }

        public bool SaveToFile(string filePath)
        {
            try
            {
                var output = new StringBuilder();
                output.AppendLine("name,code,country,lat,lon,elev,style,rwdir,rwlen,freq,desc,userdata,pics");
                foreach (var navigationPoint in _navigationPoints)
                {
                    var name = navigationPoint.Name.Replace(' ', '-');
                    output.AppendLine($"\"{name}\",\"{name}\",CZ," +
                                      $"{LatitudeToString(navigationPoint.Coordinate)}," +
                                      $"{LongitudeToString(navigationPoint.Coordinate)}," +
                                      $"{navigationPoint.Height}m,{GetWaipointType(navigationPoint.Type)},0,,{navigationPoint.Frequency},,,,");
                }
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                File.WriteAllText(filePath, output.ToString(), Encoding.GetEncoding(1252));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Save to file {filePath} failed.");
                return false;
            }

            return true;
        }

        static string GetWaipointType(NavigationPointType type)
        {
            return type switch
            {
                NavigationPointType.Town => "1",
                NavigationPointType.Airport => "2",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        static string LatitudeToString(GeoCoordinate coordinate)
        {
            var hh = (int)coordinate.Latitude;
            var mm = (coordinate.Latitude - hh) * 60;
            return $"{hh:00}{mm:00.000}N";
        }

        public string LongitudeToString(GeoCoordinate coordinate)
        {
            var hh = (int)coordinate.Longitude;
            var mm = (coordinate.Longitude - hh) * 60;
            return $"{hh:000}{mm:00.000}E";
        }
    }
}
