using System;
using System.Drawing;
using AuroraCAD_2D.Database;
using Avalonia.Skia;
using Brush = Avalonia.Media.Brush;

namespace AuroraCAD_2D.Models;

public class Line : Avalonia.Controls.Shapes.Line, Drawable{
    private Point _start;
    private Point _end;
    private Layer _layer;

    public Line(Point start, Point end){
        _start = start;
        _end = end;
        _layer = Settings.selectedLayer;
        StartPoint = new Avalonia.Point(_start.X, _start.Y);
        EndPoint = new Avalonia.Point(_end.X, _end.Y);
        Stroke = getLayer().Color;
        StrokeThickness = getLayer().LineSize;
    }

    public void replaceEnd(double x, double y){
        _end = new Point(x, y);
        EndPoint = new Avalonia.Point(_end.X, _end.Y);
    }

    public Layer Layer{
        get => _layer;
        set => _layer = value ?? throw new ArgumentNullException(nameof(value));
    }
    public Point Start{
        get => _start;
        set{
            _start = value;
            StartPoint = new Avalonia.Point(_start.X, _start.Y);
        }
    }

public Point End{
        get => _end;
        set{
            _end = value;
            EndPoint = new Avalonia.Point(_end.X, _end.Y);
        }
    }

    public Drawable.DrawableType getType(){
        return Drawable.DrawableType.LINE;
    }

    public Layer getLayer(){
        return Layer;
    }
}