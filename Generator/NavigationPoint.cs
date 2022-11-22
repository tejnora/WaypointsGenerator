using System.Device.Location;

namespace Generator;

enum NavigationPointType
{
    Town,
    Airport
}
class NavigationPoint
{
    public string Name { get; init; }
    public GeoCoordinate Coordinate { get; init; }
    public int Height { get; init; }
    public int Area { get; set; }

    public NavigationPointType Type { get; init; }
    public string Frequency { get; init; }
}