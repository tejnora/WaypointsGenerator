using CsvHelper;
using System.Globalization;

namespace Generator;

class TownsDataReader
{
    struct TownData
    {
        public string NAZEV_OBCE { get; set; }
        public string KATASTRALNI_VYMERA { get; set; }
    }

    IEnumerable<TownData> _date;

    public void Parse(string filePath)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        _date = csv.GetRecords<TownData>().ToList();
    }

    public void ApplyToNavigationPoints(List<NavigationPoint> navigationPoints)
    {
        var map2Name = new Dictionary<string, int>();
        for (var i = 0; i < navigationPoints.Count; i++)
        {
            map2Name[navigationPoints[i].Name.ToLowerInvariant()] = i;
        }
        foreach (var townData in _date)
        {
            var name = townData.NAZEV_OBCE.ToLowerInvariant();
            if (map2Name.TryGetValue(name, out int index))
            {
                var area = townData.KATASTRALNI_VYMERA;
                area=area.Replace(",", "");
                navigationPoints[index].Area = int.Parse(area);
                continue;
            }
            Console.WriteLine($"Not matched:{townData.NAZEV_OBCE}");
        }

    }
}