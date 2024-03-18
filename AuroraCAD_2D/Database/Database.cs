using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AuroraCAD_2D.Models;
using AuroraCAD_2D.Services;
using Point = AuroraCAD_2D.Models.Point;

namespace AuroraCAD_2D.Database;

public class Database{
    private static List<Point> _points = new List<Point>();
    private static List<Drawable> _lines = new List<Drawable>();

    public static void printEntitiesAmount(){
        Logger.log(string.Format("Points in database: {0}\r\nLines in database: {1}",Points.Count,Lines.Count));
    }
    public static List<Point> Points{
        get => _points;
    }
    
    public static List<Drawable> Lines{
        get => _lines;
    }

    public static void addPoint(Point point){
        if (!_points.Contains(point)){
            _points.Add(point);
            Logger.log("Point added to database: (" + point.X+","+point.Y+")");
        }
    }
    
    public static void addLine(Drawable line){
        if (!_lines.Contains(line)){
            _lines.Add(line);
            if (line.getType() == Drawable.DrawableType.LINE){
                Line l = line as Line;
                Logger.log("Line added to database: (" + l.Start.X+";"+l.Start.Y+") -> ("+l.End.X+";"+l.End.Y+")");
            } else if (line.getType() == Drawable.DrawableType.CIRCLE){
                Circle c = line as Circle;
                Logger.log(string.Format("Added circle to database. Centre:({0} ; {1}). Radius: {2}",c.Centre.X,c.Centre.Y,c.Rad));
            }
            
        }
    }
}