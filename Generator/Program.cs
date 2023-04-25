
using System.Text;

namespace Generator;

static class GeneratorApp
{
    static readonly string RepositoryPrefix = @"e:\Repozitories\waypoints-generator\Generator\Data";
    static void Main(string[] args)
    {
        var points = TownsCoordinatesReader.Parse(Path.Combine(RepositoryPrefix, @"town-coordinates.csv"));
        var townsData = new TownsDataReader();
        townsData.Parse(Path.Combine(RepositoryPrefix, @"32019921040cz.csv"));
        townsData.ApplyToNavigationPoints(points);

        points.AddRange(AirportReader.Parse(Path.Combine(RepositoryPrefix, @"airports.txt")));

        points = FilterPointsByDistance.Do(points);
        var saver = new CubFileCreater(points);
        var outputFile = Path.Combine(RepositoryPrefix, @"myWP.cup");
        saver.SaveToFile(outputFile);
        var extraPoints = File.ReadAllText(Path.Combine(RepositoryPrefix, "extra-points.cup"));
        File.AppendAllText(outputFile, extraPoints);
        var jaPoints = File.ReadAllText(Path.Combine(RepositoryPrefix, "XL_OB_JA.cup"))
            .Split("\r\n")
            .Where((n) => n.Contains(",PL,") || n.Contains(",DE,")).ToList();
        File.AppendAllText(outputFile, string.Join("\r\n", jaPoints));
    }
}