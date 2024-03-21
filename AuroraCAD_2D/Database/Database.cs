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
}