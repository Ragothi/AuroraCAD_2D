using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using AuroraCAD_2D.Models;
using AuroraCAD_2D.Services;
using Avalonia.Controls;
using Avalonia.Media;
using Brush = Avalonia.Media.Brush;
using Brushes = Avalonia.Media.Brushes;
using Point = AuroraCAD_2D.Models.Point;

namespace AuroraCAD_2D.Database;

public class Database{
    private static List<Point> _points = new List<Point>();
    private static List<Drawable> _lines = new List<Drawable>();
    private static List<Layer> _layers = new List<Layer>();

    public static void save(string fileName){
        StringBuilder sb = new StringBuilder();
        foreach (Layer layer in _layers){
            sb.Append("<LAY>").Append("\t").Append(layer.Name).Append("\t").Append(layer.Color).Append("\t").Append(layer.PointSize).Append("\t").Append(layer.LineSize)
                .Append("\r\n");
        }

        foreach (Point p in _points){
            sb.Append("<PT>").Append("\t").Append(_layers.IndexOf(p.getLayer())).Append("\t").Append(p.X).Append("\t")
                .Append(p.Y).Append("\r\n");
        }

        foreach (Drawable line in _lines){
            if (line.getType() == Drawable.DrawableType.LINE){
                Line l = line as Line;
                sb.Append("<LN>").Append("\t").Append(_layers.IndexOf(l.getLayer())).Append("\t").Append(l.Start.X)
                    .Append("\t").Append(l.Start.Y).Append("\t").Append(l.End.X).Append("\t").Append(l.End.Y).Append("\r\n");
            }
            else{
                Circle c = line as Circle;
                sb.Append("<CL>").Append("\t").Append(_layers.IndexOf(c.getLayer())).Append("\t").Append(c.Centre.X)
                    .Append("\t").Append(c.Centre.Y).Append("\t").Append(c.Rad).Append("\r\n");
            }
        }

        Logger.log(sb.ToString());
        Logger.log(fileName);
        Utils.writeTextToAFile(sb.ToString(),fileName,false);
    }

    public static void read(string fileName){
        emptyDatabase();
        string[] lines = Utils.readFile(fileName).Split("\n");
        string[] vals;
        foreach (string line in lines){
            vals = line.Split("\t");
            switch (vals[0]){
                case "<LAY>":
                    addLayer(new Layer(Brush.Parse(vals[2]),double.Parse(vals[3]),double.Parse(vals[4]),vals[1]));
                    break;
                case "<PT>":
                    Settings.selectedLayer = _layers[int.Parse(vals[1])];
                    addPoint(new Point(double.Parse(vals[2]),double.Parse(vals[3])));
                    break;
                case "<LN>":
                    Settings.selectedLayer = _layers[int.Parse(vals[1])];
                    addLine(new Line(new Point(double.Parse(vals[2]),double.Parse(vals[3])),new Point(double.Parse(vals[4]),double.Parse(vals[5]))));
                    break;
                case "<CL>":
                    Settings.selectedLayer = _layers[int.Parse(vals[1])];
                    addLine(new Circle(double.Parse(vals[2]),double.Parse(vals[3]),double.Parse(vals[4])));
                    break;
            }
        }
    }

    public static void emptyDatabase(){
        _layers.Clear();
        _points.Clear();
        _lines.Clear();
    }
    
    public static void printEntitiesAmount(){
        Logger.log(string.Format("Layers in database: {0}\r\nPoints in database: {1}\r\nLines in database: {2}",Layers.Count,Points.Count,Lines.Count));
    }
    public static List<Point> Points{
        get => _points;
    }
    
    public static List<Drawable> Lines{
        get => _lines;
    }
    
    public static List<Layer> Layers{
        get => _layers;
    }

    public static void addLayer(Layer layer){
        if (!_layers.Contains(layer)){
            _layers.Add(layer);
            Logger.log("New layer added to database");
        }
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

    public static void getDrawablesInSelection(){
        double xMin = Canvas.GetLeft(Settings.selectionBox);
        double xMax = xMin + Settings.selectionBox.Width;
        double yMin = Canvas.GetTop(Settings.selectionBox);
        double yMax = yMin + Settings.selectionBox.Height;

        foreach (Point point in _points){
            if (point.X >= xMin && point.X <= xMax && point.Y >= yMin && point.Y <= yMax){
                Settings.selectedDrawables.Add(point);
            }
        }

        foreach (Drawable line in _lines){
            if (line.getType() == Drawable.DrawableType.LINE){
                Line l = line as Line;
                Point point = l.Start;
                Point point2 = l.End;
                if ((point.X >= xMin && point.X <= xMax && point.Y >= yMin && point.Y <= yMax)
                    || (point2.X >= xMin && point2.X <= xMax && point2.Y >= yMin && point2.Y <= yMax)){
                    Settings.selectedDrawables.Add(point);
                    Settings.selectedDrawables.Add(point2);
                    Settings.selectedDrawables.Add(line);
                }
            } else if (line.getType() == Drawable.DrawableType.CIRCLE){
                Circle c = line as Circle;
                Point point = c.Centre;
                if (point.X >= xMin-c.Rad && point.X <= xMax+c.Rad && point.Y >= yMin-c.Rad && point.Y <= yMax+c.Rad){
                    Settings.selectedDrawables.Add(point);
                    Settings.selectedDrawables.Add(c);
                }
            }
        }
        
        logSelectedDrawablesContent();
        
    }

    private static void logDatabaseContent(){
        Logger.log(string.Format("Points: {0}\r\nLines: {1}\r\nCircles: {2}",
            _points.Count,
            _lines.Where(d => d.getType() == Drawable.DrawableType.LINE).Count(),
           _lines.Where(d => d.getType() == Drawable.DrawableType.CIRCLE).Count()
        ));
    }
    
    private static void logSelectedDrawablesContent(){
        Logger.log(string.Format("Points: {0}\r\nLines: {1}\r\nCircles: {2}",
            Settings.selectedDrawables.Where(d => d.getType() == Drawable.DrawableType.POINT).Count(),
            Settings.selectedDrawables.Where(d => d.getType() == Drawable.DrawableType.LINE).Count(),
            Settings.selectedDrawables.Where(d => d.getType() == Drawable.DrawableType.CIRCLE).Count()
        ));
    }

    public static void deleteAllSelected(){
        foreach (Drawable d  in Settings.selectedDrawables){
            Settings.CanvasGlobalReference.Children.Remove(d as Control);
        }

        _points.RemoveAll(d => d.getType() == Drawable.DrawableType.POINT);
        _lines.RemoveAll(l => l.getType() == Drawable.DrawableType.LINE || l.getType() == Drawable.DrawableType.CIRCLE);
        logDatabaseContent();
    }

    public static void unselectAll(){
        foreach (Drawable d in Settings.selectedDrawables){
            switch (d.getType()){
                case Drawable.DrawableType.POINT:
                    Point p = d as Point;
                    p.Fill = p.getLayer().Color;
                    break;
                case Drawable.DrawableType.LINE:
                    Line l = d as Line;
                    l.Stroke = l.getLayer().Color;
                    l.Start.Fill = l.getLayer().Color;
                    l.End.Fill = l.getLayer().Color;
                    break;
                case Drawable.DrawableType.CIRCLE:
                    Circle c = d as Circle;
                    c.Centre.Fill = c.getLayer().Color;
                    c.Stroke = c.getLayer().Color;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        Settings.selectedDrawables.Clear();
    }
}