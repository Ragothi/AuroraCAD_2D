using System.Collections.Generic;
using System.Drawing;
using AuroraCAD_2D.Services;
using Point = AuroraCAD_2D.Models.Point;

namespace AuroraCAD_2D.Database;

public class Database{
    private static List<Point> _points = new List<Point>();

    public static List<Point> Points{
        get => _points;
    }

    public static void addPoint(Point point){
        if (!_points.Contains(point)){
            _points.Add(point);
            Logger.log("Point added to database: (" + point.X+","+point.Y+")");
        }
    }
}