namespace Generator
{
    static class FilterPointsByDistance
    {
        static readonly double MaxDistance = 5000;
        public static List<NavigationPoint> Do(List<NavigationPoint> points)
        {
            points.Sort((a,b)=>
            {
                if (a.Type < b.Type) return 1;
                if (a.Type > b.Type) return -1;
                if (a.Area < b.Area) return 1;
                return a.Area > b.Area ? -1 : 0;
            });
            var result = new List<NavigationPoint>();
            if (points.Count == 0) return result;
            result.Add(points[0]);
            for (var i = 1; i < points.Count; i++)
            {
                var cPoint = points[i];
                var skipPoint = false;
                foreach (var rPoint in result)
                {
                    var distance = rPoint.Coordinate.GetDistanceTo(cPoint.Coordinate);
                    if (distance >= MaxDistance) continue;
                    skipPoint = true;
                    break;
                }
                if(skipPoint)continue;
                result.Add(cPoint);
            }

            return result;
        }
    }
}
